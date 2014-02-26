
namespace NHibernate.AutoMigration
{
    public class AlterColumnMigration : DestructiveMigration
    {
        public MigrationColumn OldColumn { get; protected set; }
        public MigrationColumn NewColumn { get; protected set; }

        public AlterColumnMigration(MigrationColumn oldColumn, MigrationColumn newColumn)
            : base(oldColumn.OwnerTable)
        {
            OldColumn = oldColumn;
            NewColumn = newColumn;
        }

        /// <summary>
        /// return up script
        /// </summary>
        /// <returns></returns>
        public override string UpScript()
        {
            return MigrationHelpers.ScriptForAlterColumn(OldColumn, NewColumn);

        }

        /// <summary>
        /// return down script
        /// </summary>
        /// <returns></returns>
        public override string DownScript()
        {
            return MigrationHelpers.ScriptForAlterColumn(NewColumn, OldColumn);
        }
    }
}
