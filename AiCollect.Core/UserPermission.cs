using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class UserPermission : AiCollectObject
    {
        private UserRight _userRight;
        private PermisionType _permission;
        private UserPermission _original;

       
        [DataMember]
        public PermisionType Permission
        {
            get
            {
                return this._permission;
            }
            set
            {
                if (this._permission == value)
                    return;
                this._permission = value;
                this.ObjectState = ObjectStates.Modified;
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

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

        private PermissionObjects _permissionObject;
        [DataMember]
        public PermissionObjects PermissionObject
        {
            get
            {
                return _permissionObject;
            }
           set
            {
                if(_permissionObject!=value)
                {
                    _permissionObject = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        public new UserPermissions Parent
        {
            get
            {
                return base.Parent as UserPermissions;
            }
        }
        [DataMember]
        public UserRight UserRight
        {
            get
            {
                return _userRight;
            }
            set
            {
                _userRight = UserRight;
            }
        }
        public UserPermission(AiCollectObject parent)
          : base(parent)
        {

            this.ObjectState = ObjectStates.None;
            this.SetOriginal();
        }

        public override void Cancel()
        {
            switch (this.ObjectState)
            {
                case ObjectStates.Added:
                    this.Parent.Remove(this);
                    break;
                case ObjectStates.Modified:
                    this._permission = this._original.Permission;
                   
                    this.SetOriginal();
                    break;
            }
        }

        internal override void SetOriginal()
        {
            this._original = this.Copy() as UserPermission;
        }

        public override void Update()
        {
            switch (this.ObjectState)
            {
                case ObjectStates.Removed:
                    this.Parent.Remove(this);
                    break;
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    this.ObjectState = ObjectStates.None;
                    this.SetOriginal();
                    break;
            }
        }

      


        public override void Validate()
        {

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            if (obj["Permission"] != null && ((JValue)obj["Permission"]).Value != null)
                Permission =(PermisionType) Enum.Parse(typeof(PermisionType),((JValue)obj["Permission"]).Value.ToString());
           
        }

    }
}
