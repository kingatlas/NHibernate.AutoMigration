
namespace NHibernate.AutoMigration
{
    public class DropTableMigration : DestructiveMigration
    {
        
        public DropTableMigration(MigrationTable table):base(table)
        {
        }

        /// <summary>
        /// return up script
        /// </summary>
        /// <returns></returns>
        public override string UpScript()
        {
            return MigrationHelpers.ScriptForDropTable(Table);

        }

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public override string DownScript()
        {
            return MigrationHelpers.ScriptForCreateTable(Table);
        }
    }
}
