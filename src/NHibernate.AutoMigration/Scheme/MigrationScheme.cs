using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHibernate.AutoMigration
{
    public class MigrationScheme : Dictionary<string, MigrationTable>
    {
        public IList<MigrationTable> Tables { get { return this.Values.ToList(); } }
 
        private static JsonSerializerSettings JsonSettings;

        static MigrationScheme()
        {
            JsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new DefaultContractResolver
                {
                    DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                },
            };

        }

        public MigrationScheme(IEnumerable<MigrationTable> tables)
            :base(tables.ToDictionary(t => t.Name))
        {
        }

        public MigrationScheme()
            :base()
        {
            
        }

        public string Serialize()
        {
           return  JsonConvert.SerializeObject(this, Formatting.Indented, JsonSettings);
        }

        public static MigrationScheme Deserialize(string json)
        {
           return  (MigrationScheme)JsonConvert.DeserializeObject(json, typeof(MigrationScheme), JsonSettings);
        }

        public MigrationScheme Clone()
        {
            return MigrationScheme.Deserialize(this.Serialize());
        }

        public bool HasTable(string tableName)
        {
            return this.ContainsKey(tableName);
        }
    }
}
