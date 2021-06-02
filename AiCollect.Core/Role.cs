using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Role : AiCollectObject
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
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    ObjectState = ObjectStates.Modified;
                   
                }
            }
        }

        private string _description;

        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        public new Roles Parent
        {
            get
            {
                return (Roles)base.Parent;
            }
        }

        protected Role OriginalValues { get; private set; }

        public Role(AiCollectObject parent)
            : base(parent)
        {
            SetOriginalValues();
        }

        protected void SetOriginalValues()
        {           
            ObjectState = ObjectStates.None;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            Name = ((JValue)obj["Name"]).Value.ToString();
            Description = ((JValue)obj["Description"]).Value.ToString();
            SetOriginalValues();
        }

        public override void Validate()
        {
            
        }

        public override void Cancel()
        {
            
        }

        public override void Update()
        {
           
        }
    }
}