using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(TraineeConverter))]
    public class Trainee : AICollect
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
        private string _farmerKey;
        [DataMember]
        public string FarmerKey
        {
            get
            {
                return _farmerKey;
            }
            set
            {
                _farmerKey = value;
            }
        }

        [DataMember]
        public Questionaire Questionaire { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string TrainingId { get; set; }

        public Trainee(AiCollectObject parent) : base(parent)
        {
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["FarmerKey"] != null && ((JValue)obj["FarmerKey"]).Value != null)
                FarmerKey = ((JValue)obj["FarmerKey"]).Value.ToString();

            if (obj["TrainingId"] != null && ((JValue)obj["TrainingId"]).Value != null)
                TrainingId = ((JValue)obj["ConfigurationId"]).Value.ToString();
        }

    }
}
