using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserRight : AiCollectObject
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
        public string PrimaryKey { get; set; }

        [DataMember]
        public string SecondaryKey { get; set; }

        private string _objectName;
        private ObjectType objectType;

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string ObjectName
        {
            get { return _objectName; }
            set
            {
                if (value != _objectName)
                {
                    _objectName = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        [Browsable(false)]
        public ObjectType ObjectType { get => objectType; set => objectType = value; }

        private bool _isSystem;
        /// <summary>
        /// Gets/Sets Whether the card is a system card
        /// </summary>
        [DataMember]
        public bool IsSystem
        {
            get
            {
                return _isSystem;
            }
            set
            {
                if (_isSystem != value)
                {
                    _isSystem = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        
        [DataMember]
        public int UserId { get; set; }

        public new UserRights Parent
        {
            get
            {
                return (UserRights)base.Parent;
            }
        }

        [DataMember]
        public new Configuration Configuration { get; set; }

        protected UserRight OriginalValues { get; private set; }

        [DataMember]
        public UserPermissions UserPermissions
        {
            get;set;
        }

        public UserRight(AiCollectObject parent)
            : base(parent)
        {
            UserPermissions = new UserPermissions(this);
        }


        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["ObjectName"] != null && ((JValue)obj["ObjectName"]).Value != null)
                ObjectName = ((JValue)obj["ObjectName"]).Value.ToString();
            if (obj["IsSystem"] != null && ((JValue)obj["IsSystem"]).Value != null)
                IsSystem = bool.Parse(((JValue)obj["IsSystem"]).Value.ToString());
            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

            if (obj["UserPermissions"] != null)
            {
                JArray userRightsObj = JArray.FromObject(obj["UserPermissions"]);

                if (userRightsObj != null)
                {
                    UserPermissions.ReadJson(obj);
                }
            }

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

        public override int CompareTo(AiCollectObject other)
        {
            UserRight userRight = this;
            UserRight otherUserRight = other as UserRight;
            var sameName = userRight.ObjectName.Equals(otherUserRight.ObjectName);
            return sameName ? 1 : 0;
        }
    }
}
