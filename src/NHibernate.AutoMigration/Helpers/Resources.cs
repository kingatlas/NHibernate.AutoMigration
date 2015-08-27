using System.IO;
using System.Reflection;

namespace NHibernate.AutoMigration.Helpers
{
    /// <summary>
    /// Resources Helper
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// Read the resource as a string.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string ReadString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
