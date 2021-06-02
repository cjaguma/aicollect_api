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
    [JsonConverter(typeof(TopicConverter))]
    public class Topic : AiCollectObject
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

        private string _name;
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name!=value)
                {
                    _name = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string TrainingId { get; set; }
        public Topic(AiCollectObject parent) : base(parent)
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

            if (obj["TrainingId"] != null && ((JValue)obj["TrainingId"]).Value != null)
                TrainingId = ((JValue)obj["TrainingId"]).Value.ToString();
        }

    }
}
