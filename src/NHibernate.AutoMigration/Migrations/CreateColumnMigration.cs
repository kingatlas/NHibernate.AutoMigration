
namespace NHibernate.AutoMigration
{
    public class CreateColumnMigration : ConstructiveMigration
    {
        public MigrationColumn Column { get; protected set; }

        public CreateColumnMigration(MigrationColumn column)
            :base(column.OwnerTable)
        {
            Column = column;
        }

        /// <summary>
        /// return up script
        /// </summary>
        /// <returns></returns>
        public override string UpScript()
        {
            return MigrationHelpers.ScriptForCreateColumn(Column);

        }

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public override string DownScript()
        {
            return MigrationHelpers.ScriptForDropColumn(Column);
        }
    }
}
