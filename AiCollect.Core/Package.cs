using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(PackageConverter))]
    public class Package : AiCollectObject
    {
        [DataMember]
        public new string Key
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }

        [DataMember]
        public new int OID
        {
            get
            {
                return base.OID;
            }
            set
            {
                base.OID = value;
            }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Plan Plan { get; set; }
        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public decimal Price { get; set; }
        
        public Package()
        {
        }

        public Package(AiCollectObject parent) : base(parent)
        {
        }

        public override void Cancel()
        {
           
        }

        public override void Update()
        {
            
        }

        public override void Validate()
        {
           
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();

            if (obj["Plan"] != null && ((JValue)obj["Plan"]).Value != null)
                Plan = (Plan) Enum.Parse(typeof(Plan),((JValue)obj["Plan"]).Value.ToString());

            if (obj["Price"] != null && ((JValue)obj["Price"]).Value != null)
                Price = Convert.ToDecimal(((JValue)obj["Price"]).Value);

        }
    }
}
