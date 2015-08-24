using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.AutoMigration.Files;
using System.Text;

namespace NHibernate.AutoMigration
{
   public class MigrationGenerator
    {

        public string PreviousSnapshotFileName { get; protected set; }

        public NHibernate.Cfg.Configuration Configuration { get; protected set; }

        public MigrationScheme CurrentScheme { get; protected set; }



        public MigrationGenerator(NHibernate.Cfg.Configuration configuration)
        {
            Configuration = configuration;
            Configuration.BuildMappings(); // necessary to complete columns creation (foreign keys...)

            var mapping = Configuration.CreateMappings(new GenericDialect());
            // extract the tables scheme
            CurrentScheme = new MigrationScheme(mapping.IterateTables.Select(t => new MigrationTable(t)));
            UpdateForeignKeys();

            PreviousSnapshotFileName = string.Format("{0}\\previous.snapshot",Directory.GetCurrentDirectory());

        }


        private void UpdateForeignKeys()
        {
            var allforeignKeys =CurrentScheme.Tables.SelectMany(t => t.ForeignKeyColumns);

            foreach (var foreignKeyColumn in allforeignKeys)
            {
                foreignKeyColumn.ReferencedTable = CurrentScheme[foreignKeyColumn.ReferencedTableName];
            }
        }

        /// <summary>
        /// return a JSON representation of the current tables scheme
        /// </summary>
        /// <returns></returns>
        public string Snapshot()
        {
            return CurrentScheme.Serialize();
        }

        public void SaveSnapshot()
        {
            var snapshot = Snapshot();
            
            // delete the old file
            if (File.Exists(PreviousSnapshotFileName))
                File.Delete(PreviousSnapshotFileName);
            
            // write the new snapshot
            File.WriteAllText(PreviousSnapshotFileName, snapshot);
        }

        public MigrationScheme GetPreviousScheme()
        {
            if (File.Exists(PreviousSnapshotFileName))
            {
                var previous_snapshot_string = File.ReadAllText(PreviousSnapshotFileName);
                return MigrationScheme.Deserialize(previous_snapshot_string);
            }
            else
            {
                return new MigrationScheme(); // empty configuration
            }
        }

        public IEnumerable<Migration> GenerateMigration()
        {
            var previous_scheme = GetPreviousScheme();

            return CompareScheme(previous_scheme, CurrentScheme).ToList();

        }

       public IEnumerable<MigrationFile> GenerateMigrationFiles()
       {
           var allMigrations = GenerateMigration();
           var migrationsByTable = allMigrations.GroupBy(m => m.Table.Name).ToDictionary(g => g.Key, g => g.ToList());

           foreach (var tableName in migrationsByTable.Keys)
           {
                var filename = string.Format("Migration_{0}.cs", tableName);
                var migrationFile = new MigrationCsFile(filename, new ASCIIEncoding());
             
               var upscript = string.Join(System.Environment.NewLine, migrationsByTable[tableName].Select(m => m.UpScript()));
               var downscript = string.Join(System.Environment.NewLine, migrationsByTable[tableName].Select(m => m.DownScript())); // todo invert the list

               

               yield return migrationFile;
           }
       }

       public  IEnumerable<Migration> CompareScheme(MigrationScheme previous, MigrationScheme current)
        {
            // detect new tables
            foreach (var table in current.Tables)
            {
                if (!previous.HasTable(table.Name))
                {
                    yield return new CreateTableMigration(table);
                }
                else // table still exists
                { 
                    var previous_table = previous[table.Name];

                    foreach (var column in table.Columns)
                    {
                        var previous_column = previous_table.Columns.FirstOrDefault(c => c.Name == column.Name);
                        if (previous_column == null) //detect new columns
                        {
                            yield return new CreateColumnMigration(column);
                        }
                        else
                        { 
                            // column alteration
                            if (!previous_column.IsSimilarTo(column))
                                yield return new AlterColumnMigration(previous_column, column);
                        }
                    }

                    // delect deleted columns
                    foreach (var previous_column in previous_table.Columns)
                    {
                        var column = table.Columns.FirstOrDefault(c => c.Name == previous_column.Name);
                        if (column == null)
                        { // column was deleted 
                            yield return new DeleteColumnMigration(previous_column);
                        }
                    }
                }
            }

            // detect droped tables
            foreach (var table in previous.Tables)
            {
                if (!current.HasTable(table.Name))
                {
                    yield return new DropTableMigration(table);
                }
            }
        }


        public void CommitMigration()
        {
            SaveSnapshot();
        }
    }
}
