using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping;

namespace NHibernate.AutoMigration
{
    public class MigrationTable 
    {
        public virtual string Name { get; protected set; }
        
        /// <summary>
        /// All the columns (including foreign keys and primary keys)
        /// </summary>
        public virtual List<MigrationColumn> Columns { get; protected set; }
        public virtual IEnumerable<ForeignKeyColumn> ForeignKeyColumns { get { return Columns.Where(c => c is ForeignKeyColumn).Select(c => c as ForeignKeyColumn); } }

        public virtual MigrationColumn PrimaryKeyColumn {
            get { return Columns.FirstOrDefault(c => c.IsPrimaryKey); }
        }

        public MigrationTable()
        {
            Columns = new List<MigrationColumn>();

        }

        public MigrationTable(string tableName, IEnumerable<MigrationColumn> columns)
        {
            Columns = columns.ToList();
            Name = tableName;


        }

        public MigrationTable(Table table)
        {
            var migrationColumnFactory = new MigrationColumnFactory(this);
            Columns = table.ColumnIterator.Select(c => migrationColumnFactory.Create(c)).ToList();
            var foreignKeyColumns = table.ForeignKeyIterator.Select(c => migrationColumnFactory.Create(c)).ToList();
            Columns = Columns.Where(c => !foreignKeyColumns.Any(fk => fk.Name == c.Name)).ToList();
            Columns.AddRange(foreignKeyColumns);

            if (table.PrimaryKey != null)
            {
                var primaryColumns = Columns.Where(c => table.PrimaryKey.Columns.Any(cc => cc.Name == c.Name)).First();  // we have only one primary key column
                 primaryColumns.SetAsPrimaryKey(); 
                
            }

            Name = table.Name;
        }
    }
}
