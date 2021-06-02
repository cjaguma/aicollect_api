using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Roles : AiCollectObject, IEnumerable<Role>,ICollection<Role>
    {
        private List<Role> _roles;

        public int Count
        {
            get
            {
                return _roles.Count;
            }
        }

        public new User Parent
        {
            get
            {
                return (User)base.Parent;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Roles(AiCollectObject parent)
            : base(parent)
        {
            _roles = new List<Role>();
        }

        public Role Add()
        {
            Role role = new Role(this);
           
            _roles.Add(role);

            return role;
        }

       
        public IEnumerator<Role> GetEnumerator()
        {
            foreach (Role role in _roles)
            {
                if (role.ObjectState != ObjectStates.Removed)
                    yield return role;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Role ByName(string name)
        {
            return _roles.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _roles.Clear();
            JArray rolesObj = JArray.FromObject(obj["Roles"]);
            if (rolesObj != null)
            {
                foreach (JObject roleObj in rolesObj)
                {
                    var role = Add();
                    role.ReadJson(roleObj);
                }
            }
        }

   

        internal void InternalRemove(Role role)
        {
            _roles.Remove(role);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int max = _roles.Count;
            int i = 0;
            foreach (Role r in _roles)
            {
                if (i < max)
                    sb.Append(string.Format("{0},", r.Name));
                else
                    sb.Append(string.Format("{0}", r.Name));
                i++;
            }
            return sb.ToString();
        }

        public override int CompareTo(AiCollectObject other)
        {
            if ((other as Roles).Count != this.Count)
            {
                return 1;
            }
            else
                return 0;
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

        public void Add(Role item)
        {
            _roles.Add(item);
        }

        public void Clear()
        {
            _roles.Clear();
        }

        public bool Contains(Role item)
        {
            return _roles.Contains(item);
        }

        public void CopyTo(Role[] array, int arrayIndex)
        {
            
        }

        public bool Remove(Role item)
        {
            return _roles.Remove(item);
        }
    }
}