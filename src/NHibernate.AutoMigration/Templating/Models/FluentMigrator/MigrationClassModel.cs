namespace NHibernate.AutoMigration.Templating.Models.FluentMigrator
{
    /// <summary>
    /// Model user to render a migration class
    /// </summary>
    public class MigrationClassModel
    {
        /// <summary>
        /// Gets or sets the name of the migration class.
        /// </summary>
        public string ClassName { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the base migration class.
        /// </summary>
        public string BaseClassName { get; protected set; }

        /// <summary>
        /// Gets or sets the migration up code.
        /// </summary>
        public string UpCode { get; protected set; }

        /// <summary>
        /// Gets or sets the migration down code.
        /// </summary>
        public string DownCode { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationClassModel"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="baseClassName">Name of the base class.</param>
        /// <param name="upCode">Migration Up code.</param>
        /// <param name="downCode">Migration Down code.</param>
        public MigrationClassModel(string className, string baseClassName, string upCode, string downCode)
        {
            this.ClassName = className;
            this.BaseClassName = baseClassName;
            this.UpCode = upCode;
            this.DownCode = downCode;
        }
    }
}