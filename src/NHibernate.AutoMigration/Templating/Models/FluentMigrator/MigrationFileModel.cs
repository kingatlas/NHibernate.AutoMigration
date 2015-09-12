using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AutoMigration.Templating.Models.FluentMigrator
{
    /// <summary>
    /// Model used to render on the migration file
    /// </summary>
    public class MigrationFileModel
    {
        public IEnumerable<string> Migrations { get; protected set; }

        /// <summary>
        /// Gets or sets the migration class namespace.
        /// </summary>
        /// <value>
        /// The migration class namespace.
        /// </value>
        public string Namespace { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationFileModel"/> class.
        /// </summary>
        /// <param name="namespace">The namespace of the migration class.</param>
        /// <param name="migrations">the migrations models</param>
        public MigrationFileModel(string @namespace, IEnumerable<string> migrations)
        {
            this.Migrations = migrations;
            this.Namespace = @namespace;           
        }
    }
}
