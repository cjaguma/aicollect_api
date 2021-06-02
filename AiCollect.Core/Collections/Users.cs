using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Users : AiCollectObject, IEnumerable<User>, ICollection<User>
    {
        private List<User> _users;

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public new IUserParent Parent
        {
            get
            {
                return (IUserParent)base.Parent;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        int ICollection<User>.Count
        {
            get
            {
                return _users.Count;
            }
        }

        bool ICollection<User>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Users(AiCollectObject parent)
            : base(parent)
        {
            _users = new List<User>();
        }

        public User ByName(string name)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool Exists(string name)
        {
            return ByName(name) != null ? true : false;
        }

        public User Add(UserTypes userType)
        {
            User user = new User(this);
            user.ObjectState = ObjectStates.Added;
            user.UserType = userType;
            _users.Add(user);
            return user;
        }


        public IEnumerator<User> GetEnumerator()
        {
            foreach (User user in _users)
            {
                if (user.ObjectState != ObjectStates.Removed)
                    yield return user;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _users.Clear();
            JArray usersObj = JArray.FromObject(obj["Users"]);
            if (usersObj != null)
            {
                foreach (var userObj in usersObj)
                {
                    var item = Add(UserTypes.ClientUser);
                    item.ReadJson((JObject)userObj);
                }
            }
        }



        internal void InternalRemove(User user)
        {
            _users.Remove(user);
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

        public void Add(User item)
        {
            _users.Add(item);
        }

        public void Clear()
        {
            _users.Clear();
        }

        public bool Contains(User item)
        {
            return _users.Contains(item);
        }

        public void CopyTo(User[] array, int arrayIndex)
        {

        }

        public bool Remove(User item)
        {
            return _users.Remove(item);
        }

        void ICollection<User>.Add(User item)
        {
            _users.Add(item);
        }

        void ICollection<User>.Clear()
        {
            _users.Clear();
        }

        bool ICollection<User>.Contains(User item)
        {
            return _users.Contains(item);
        }

        void ICollection<User>.CopyTo(User[] array, int arrayIndex)
        {

        }

        bool ICollection<User>.Remove(User item)
        {
            return _users.Remove(item);
        }

        IEnumerator<User> IEnumerable<User>.GetEnumerator()
        {
            foreach (var u in _users)
                yield return u;
        }

    }
}