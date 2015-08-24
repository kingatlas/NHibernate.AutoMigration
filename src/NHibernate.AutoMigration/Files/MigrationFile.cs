
using System.IO;
using System.Text;

namespace NHibernate.AutoMigration.Files
{
    /// <summary>
    /// Generated migration file
    /// </summary>
    public abstract class MigrationFile 
    {
        /// <summary>
        /// The encoding used by the file
        /// </summary>
        public readonly Encoding Encoding;

        /// <summary>
        /// The content stream of the file
        /// </summary>
        public readonly Stream Content;

        /// <summary>
        /// The file name of the file
        /// </summary>
        public readonly string FileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="encoding">the text encoding</param>
        public MigrationFile(string fileName, Encoding encoding)
        {
            this.Encoding = encoding;
            this.FileName = fileName;
            this.Content = new MemoryStream();
        }

        /// <summary>
        /// Appends the specified content to the current file content
        /// </summary>
        /// <param name="content">The content to append</param>
        public void Append(string content)
        {
            var bytes = this.Encoding.GetBytes(content);
            this.Content.Write(bytes, 0, bytes.Length);
        }
    }
}
