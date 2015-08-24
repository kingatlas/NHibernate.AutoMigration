using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AutoMigration.Templating.Models.FluentMigrator
{
    public class MigrationFileModel
    {
        public bool IsInherited { get; set; }

        public string ClassDeclaration { get; set; }

        public string ClassAttribute { get; set; }

        public string ClassName { get; set; }

        public string BaseClassName { get; set; }

        public string UpCode { get; set; }

        public string DownCode { get; set; }

        public string Namespace { get; set; }
    }
}
