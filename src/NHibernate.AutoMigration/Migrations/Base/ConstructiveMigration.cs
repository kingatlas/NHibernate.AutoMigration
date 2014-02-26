
namespace NHibernate.AutoMigration
{
    public abstract class ConstructiveMigration : Migration
    {
        public ConstructiveMigration(MigrationTable table):base(table)
        {

        }
    }
}
