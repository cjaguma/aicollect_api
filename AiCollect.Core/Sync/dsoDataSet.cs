
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core.Sync
{
    [DataContract]
   // [JsonConverter(typeof(dsoDataSetConverter))]
    public class dsoDataSet
    {
        [DataMember]
        public string Database { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public dsoDataTables Tables { get; set; }

        public dsoDataSet()
        {
            Tables = new dsoDataTables();
        }


        public string ToJsonString()
        {
            string json = JsonConvert.SerializeObject(this); 
            return json;
        }
    }
}