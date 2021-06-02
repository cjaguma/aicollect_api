using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public class UserPermissions : AiCollectObject, IEnumerable<UserPermission>
    {
        private List<UserPermission> _permissions;

        public new User Parent
        {
            get
            {
                return base.Parent as User;
            }
        }

        public int Count
        {
            get
            {
                return this._permissions.Count;
            }
        }

        public UserPermission this[int index]
        {
            get
            {
                return this._permissions[index];
            }
        }

        public UserPermissions(AiCollectObject parent)
          : base(parent)
        {
            this._permissions = new List<UserPermission>();
        }

        public UserPermission Add()
        {
            UserPermission userPermission = new UserPermission(this);
            userPermission.Configuration = new Configuration();
            userPermission.ObjectState = ObjectStates.Added;
            this._permissions.Add(userPermission);
            return userPermission;
        }

        public UserPermission ByUserRight(UserRight userRight)
        {
            return this._permissions.FirstOrDefault<UserPermission>((Func<UserPermission, bool>)(p =>
            {
                if (p.UserRight != null)
                    return p.UserRight.Equals((object)userRight);
                return false;
            }));
        }

        internal void Remove(UserPermission permission)
        {
            this._permissions.Remove(permission);
        }

        public override void Cancel()
        {
            for (int index = this._permissions.Count - 1; index >= 0; --index)
                this._permissions[index].Cancel();
        }

        public IEnumerator<UserPermission> GetEnumerator()
        {
            foreach (UserPermission permission in this._permissions)
                yield return permission;
        }

        public override void Update()
        {
            for (int index = this._permissions.Count - 1; index >= 0; --index)
                this._permissions[index].Update();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        internal void Clear()
        {
            this._permissions.Clear();
        }

        public override void Validate()
        {
            
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            JArray permissions = null;
            if (obj["UserPermissions"] != null)
            {
                permissions = JArray.FromObject(obj["UserPermissions"]);
            }
            
            _permissions.Clear();
            foreach(JObject objP in permissions)
            {
                UserPermission userPermission = Add();
                userPermission.ReadJson(objP);
            }
        }
    }
}
