using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace AiCollect.Core
{
    [DataContract]
    public class Permissions : AiCollectObject, IEnumerable<Permission>,ICollection<Permission>
    {
        private List<Permission> _permissions;

        public int Count
        {
            get
            {
                return _permissions.Count;
            }
        }

        public new UserRight Parent
        {
            get
            {
                return (UserRight)base.Parent;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Permissions(AiCollectObject parent)
            : base(parent)
        {
            _permissions = new List<Permission>();
        }

        public Permissions():base(null)
        {

        }

        public Permission ByType(PermissionTypes type)
        {
            return _permissions.FirstOrDefault(a => a.Type == type);
        }

        public bool Exists(PermissionTypes type)
        {
            return ByType(type) != null ? true : false;
        }

        public Permission Add()
        {
            return Add(PermissionTypes.Read);
        }

        public Permission Add(PermissionTypes type)
        {
            Permission permission = new Permission(this);
            permission.ObjectState = ObjectStates.Added;
            permission.Type = type;
          
            _permissions.Add(permission);

            return permission;
        }

        public Permission AddRead()
        {
            return Add(PermissionTypes.Read);
        }

        public Permission AddWrite()
        {
            return Add(PermissionTypes.Write);
        }

        public Permission AddDelete()
        {
            return Add(PermissionTypes.Delete);
        }

      

        public IEnumerator<Permission> GetEnumerator()
        {
            foreach (Permission permission in _permissions)
            {
                if (permission.ObjectState != ObjectStates.Removed)
                    yield return permission;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _permissions.Clear();
            JArray permissionsObj = JArray.FromObject(obj["Permissions"]);
            if (permissionsObj!=null)
            {
                foreach(JObject permissionObj in permissionsObj)
                {
                    if ((JValue)obj["Type"]!=null&& ((JValue)obj["Type"]).Value!=null)
                    {
                        var type = (PermissionTypes)Enum.Parse(typeof(PermissionTypes), ((JValue)obj["Type"]).Value.ToString());
                        var permission = Add(type);
                        permission.ReadJson(permissionObj);
                    }  
                }
            }
        }

       
        internal void InternalRemove(Permission permission)
        {
            _permissions.Remove(permission);
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

        public void Add(Permission item)
        {
            _permissions.Add(item);
        }

        public void Clear()
        {
            _permissions.Clear();
        }

        public bool Contains(Permission item)
        {
            return _permissions.Contains(item);
        }

        public void CopyTo(Permission[] array, int arrayIndex)
        {
            
        }

        public bool Remove(Permission item)
        {
            return _permissions.Remove(item);
        }

    }
}