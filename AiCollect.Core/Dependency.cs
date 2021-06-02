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
    [JsonConverter(typeof(DependencyConverter))]
    public class Dependency : AiCollectObject
    {

        private Target _target;
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
        public DataCollectionObectTypes TargetObjectType { get; set; }

        public new Dependencies Parent
        {
            get
            {
                return base.Parent as Dependencies;
            }
        }

        [DataMember]
        public Target Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public string TargetObjectKey
        {
            get;set;
        }

        public string Template { get; set; }

        [DataMember]
        public string QuestionKey { get; set; }

        public Dependency(AiCollectObject parent) : base(parent)
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

            if (obj["TargetObjectType"] != null && ((JValue)obj["TargetObjectType"]).Value != null)
                TargetObjectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes),((JValue)obj["TargetObjectType"]).Value.ToString());

            if (obj["TargetObjectKey"] != null && ((JValue)obj["TargetObjectKey"]).Value != null)
                TargetObjectKey = ((JValue)obj["TargetObjectKey"]).Value.ToString();

            if (obj["Target"] != null)
            {
                JObject targetObj = JObject.FromObject(obj["Target"]);
                if (targetObj != null)
                {
                    //Target = new Target();
                    //switch (TargetObjectType)
                    //{
                    //    case DataCollectionObectTypes.Section:
                    //        Target.Section.ReadJson(JObject.FromObject(targetObj["Section"]));
                    //        break;
                    //    case DataCollectionObectTypes.SubSection:
                    //        Target.SubSection.ReadJson(JObject.FromObject(targetObj["SubSection"]));
                    //        break;
                    //}
                }
            }

            
        }


    }
}
