using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Language : AiCollectObject
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
                }
            }
        }
        private string _code;

        [DataMember]
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != _code)
                {
                    _code = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        private bool _isSystem;

        [DataMember]
        public bool IsSystem
        {
            get { return _isSystem; }
            set
            {
                if (value != _isSystem)
                {
                    _isSystem = value;
                    ObjectState = ObjectStates.Modified;                  
                }
            }
        }

        private bool _isDefault;

        [DataMember]
        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                if (value != _isDefault)
                {
                    _isDefault = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
   
        public Language(AiCollectObject parent) : base(parent)
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

        public override JObject ToJson()
        {
            JObject jObject =  base.ToJson();
            jObject.Add("Name", Name);
            jObject.Add("Code", Code);
            jObject.Add("IsSystem", IsSystem.ToString());
            jObject.Add("IsDefault", IsDefault.ToString());
            return jObject;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            Name = ((JValue)obj["Name"]).Value.ToString();
            Code = ((JValue)obj["Code"]).Value.ToString();
            IsSystem = bool.Parse(((JValue)obj["IsSystem"]).Value.ToString());
            IsDefault = bool.Parse(((JValue)obj["IsDefault"]).Value.ToString());
        }

    }
}
