using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class UserRights : AiCollectObject,IEnumerable<UserRight>,ICollection<UserRight>
    {

        private List<UserRight> _rights;

        public int Count
        {
            get
            {
                return _rights.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        int ICollection<UserRight>.Count
        {
            get
            {
                return _rights.Count;
            }
        }

        bool ICollection<UserRight>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public UserRights(AiCollectObject parent)
            : base(parent)
        {
            _rights = new List<UserRight>();

            if (parent is Configuration)
                AddSystemUserRights();
        }

        public UserRight this[string name]
        {
            get
            {
                return ByName(name);
            }
        }

        public UserRight this[int index]
        {
            get
            {
                return _rights[index];
            }
        }

        public UserRight ByName(string name)
        {
            return _rights.FirstOrDefault(a => a.ObjectName == name);
        }

        public UserRight ByKey(string key)
        {
            return _rights.FirstOrDefault(a => a.Key == key);
        }

        public bool Exists(string name)
        {
            return ByName(name) != null ? true : false;
        }

        public UserRight Add()
        {
            UserRight right = new UserRight(this);
            right.ObjectState = ObjectStates.Added;
          
            _rights.Add(right);

            return right;
        }

      
        public IEnumerator<UserRight> GetEnumerator()
        {
            foreach (UserRight right in _rights)
            {
                if (right.ObjectState != ObjectStates.Removed)
                    yield return right;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _rights.Clear();
            JArray permissionsObj = JArray.FromObject(obj["UserRights"]);
            foreach (JObject usrRightObj in permissionsObj)
            {
                var userRight = Add();
                userRight.ReadJson(usrRightObj);
            }
        }

     

        internal void InternalRemove(UserRight right)
        {
            _rights.Remove(right);
        }

        private void AddSystemUserRights()
        {
            //Device Management
            UserRight right = Add();
            right.ObjectName = "Device Management";
            right.IsSystem = true;
            //right.Update();

            //User Management
            right = Add();
            right.ObjectName = "User Management";
            right.IsSystem = true;
           // right.Update();

            //UserRights Management
            right = Add();
            right.ObjectName = "User Rights Management";
            right.IsSystem = true;
            //right.Update();

            //Group Management
            right = Add();
            right.ObjectName = "Group Management";
            right.IsSystem = true;
          //  right.Update();

            //Audit
            right = Add();
            right.ObjectName = "Audit";
            right.IsSystem = true;
          //  right.Update();
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

        public void Add(UserRight item)
        {
            _rights.Add(item);
        }

        public void Clear()
        {
            _rights.Clear();
        }

        public bool Contains(UserRight item)
        {
            return _rights.Contains(item);
        }

        public void CopyTo(UserRight[] array, int arrayIndex)
        {
           
        }

        public bool Remove(UserRight item)
        {
            return _rights.Remove(item);
        }

      

       

       
    }
}
