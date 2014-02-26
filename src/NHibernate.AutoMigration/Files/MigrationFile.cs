using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AutoMigration
{
    public class MigrationFile 
    {
        public string FileName { get; protected set; }
        public string Content { get; protected set; }

        public MigrationFile(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
