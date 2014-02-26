
namespace NHibernate.AutoMigration
{
    public abstract class Migration
    {
        /// <summary>
        /// Related table
        /// </summary>
        public MigrationTable Table { get; protected set; }

        public Migration(MigrationTable table)
        {
            Table = table;
        }

        /// <summary>
        /// return up script
        /// </summary>
        /// <returns></returns>
        public abstract string UpScript();

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public abstract string DownScript();


    }
}
