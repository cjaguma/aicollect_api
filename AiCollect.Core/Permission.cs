using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [Flags]
    public enum PermissionTypes
    {
        Read = 0x0,
        Write = 0x1,
        Delete = 0x2,
        Edit = 0x4
    }

    [DataContract]
    public class Permission : AiCollectObject
    {
        private PermissionTypes _type;

        [DataMember]
        public PermissionTypes Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    ObjectState = ObjectStates.Modified;
                    
                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        public new Permissions Parent
        {
            get
            {
                return (Permissions)base.Parent;
            }
        }

        protected Permission OriginalValues { get; private set; }

        public Permission(AiCollectObject parent)
            : base(parent)
        {
            SetOriginalValues();
        }

        protected void SetOriginalValues()
        {
            //OriginalValues = (Permission)Copy();
            ObjectState = ObjectStates.Modified;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            Type = (PermissionTypes)Enum.Parse(typeof(PermissionTypes), ((JValue)obj["Type"]).Value.ToString());
            SetOriginalValues();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}