
namespace NHibernate.AutoMigration
{
    public class CreateTableMigration : ConstructiveMigration
    {
       

        public CreateTableMigration(MigrationTable table)
            :base(table)
        {
           
        }

        /// <summary>
        /// return up script
        /// </summary>
        /// <returns></returns>
        public override string UpScript()
        {
            return MigrationHelpers.ScriptForCreateTable(Table);

        }

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public override string DownScript()
        {
            return MigrationHelpers.ScriptForDropTable(Table);
        }
    }
}
