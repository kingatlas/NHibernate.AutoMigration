

using System;
using System.IO;
using System.Text;

namespace NHibernate.AutoMigration.Files
{
    /// <summary>
    /// Generated .cs migration file
    /// </summary>
    public class MigrationCsFile : MigrationFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationCsFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="encoding">the text encoding</param>
        public MigrationCsFile(string fileName, Encoding encoding): base(fileName, encoding)
        {
            var extension = Path.GetExtension(fileName);

            if (extension != "cs")
                throw new ArgumentException("The file extension must be .cs", "fileName");

        }

    }
}
