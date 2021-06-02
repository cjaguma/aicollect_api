using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(DataLinkConverter))]
    public class DataLink : DataField
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string OriginObject { get; set; }
        [DataMember]
        public string ReferredObject { get; set; }
        [DataMember]
        public Multiplicities Multiplicity { get; set; }

        public DataLink()
        {

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Multiplicity"] != null && ((JValue)obj["Multiplicity"]).Value != null)
                Multiplicity = (Multiplicities)Enum.Parse(typeof(Multiplicities), ((JValue)obj["Multiplicity"]).Value.ToString());
            if (obj["OriginObject"] != null && ((JValue)obj["OriginObject"]).Value != null)
                OriginObject = ((JValue)obj["OriginObject"]).Value.ToString();
            if (obj["ReferredObject"] != null && ((JValue)obj["ReferredObject"]).Value != null)
                ReferredObject = ((JValue)obj["ReferredObject"]).Value.ToString();
            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();
        }

    }
}
