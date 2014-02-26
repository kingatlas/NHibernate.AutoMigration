
namespace NHibernate.AutoMigration
{
    public class DeleteColumnMigration : DestructiveMigration
    {
        public MigrationColumn Column { get; protected set; }

        public DeleteColumnMigration(MigrationColumn column)
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
            return MigrationHelpers.ScriptForDropColumn(Column);

        }

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public override string DownScript()
        {
            return MigrationHelpers.ScriptForCreateColumn(Column);
        }
    }
}
