
namespace NHibernate.AutoMigration
{
    public abstract class DestructiveMigration : Migration
    {
        public DestructiveMigration(MigrationTable table)
            : base(table)
        {

        }
    }
}
