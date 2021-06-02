using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(UserConverter))]
    public class User : AiCollectObject
    {
        #region Members
        private string _firstname;
        private string _lastname;
        private string _userName;
        private string _usercode;
        private UserPermissions _permissions;
        private UserRights _userRights;
        #endregion
        [DataMember]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public string Password { get; set; }

        private string _email;

        [DataMember]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }
        
        [DataMember]
        public bool Enabled { get; set; }

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
        public string ClientId { get; set; }

        [DataMember]
        public string ConfigurationId { get; set; }

        public Roles Roles { get; private set; }

        public new Users Parent
        {
            get
            {
                return (Users)base.Parent;
            }
        }
        [DataMember]
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                if (value != _firstname)
                {
                    _firstname = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (value != _lastname)
                {
                    _lastname = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public string Usercode
        {
            get
            {
                return _usercode;
            }
            set
            {
                if (value != _usercode)
                {
                    _usercode = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }



        [DataMember]
        public UserRights UserRights
        {
            get
            {
                return _userRights;
            }
            set
            {
                _userRights = value;
            }
        }

        private bool _isAdmin;
        [DataMember]
        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                }
            }
        }

        private UserTypes _userType;
        [DataMember]
        public UserTypes UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                if (_userType != value)
                {
                    _userType = value;
                }
            }
        }

        private UserStatuses _status;
        [DataMember]
        public UserStatuses Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                }
            }
        }

        public User(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        public User() : base()
        {
            Init();
        }

        private void Init()
        {
            UserRights = new UserRights(this);
        }

        public override void Validate()
        {

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            try
            {
                if (obj["UserType"] != null && ((JValue)obj["UserType"]).Value != null)
                    UserType = (UserTypes)Enum.Parse(typeof(UserTypes), ((JValue)obj["UserType"]).Value.ToString());

                if (obj["Status"] != null && ((JValue)obj["Status"]).Value != null)
                    Status = (UserStatuses)Enum.Parse(typeof(UserStatuses), ((JValue)obj["Status"]).Value.ToString());

                if (obj["Firstname"] != null && ((JValue)obj["Firstname"]).Value != null)
                    Firstname = ((JValue)obj["Firstname"]).Value.ToString();

                if (obj["Email"] != null && ((JValue)obj["Email"]).Value != null)
                    Email = ((JValue)obj["Email"]).Value.ToString();

                if (obj["Lastname"] != null && ((JValue)obj["Lastname"]).Value != null)
                    Lastname = ((JValue)obj["Lastname"]).Value.ToString();

                if (obj["IsAdmin"] != null && ((JValue)obj["IsAdmin"]).Value != null)
                    IsAdmin = bool.Parse(((JValue)obj["IsAdmin"]).Value.ToString());

                if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                    Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

                if (obj["Enabled"] != null && ((JValue)obj["Enabled"]).Value != null)
                    Enabled = bool.Parse(((JValue)obj["Enabled"]).Value.ToString());

                if (obj["Password"] != null && ((JValue)obj["Password"]).Value != null)
                    Password = ((JValue)obj["Password"]).Value.ToString();

                if (obj["UserName"] != null && ((JValue)obj["UserName"]).Value != null)
                    UserName = ((JValue)obj["UserName"]).Value.ToString();
                
                if (obj["Usercode"] != null && ((JValue)obj["Usercode"]).Value != null)
                    Usercode = ((JValue)obj["Usercode"]).Value.ToString();

                if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
                    ConfigurationId = ((JValue)obj["ConfigurationId"]).Value.ToString();

                if (obj["ClientId"] != null && ((JValue)obj["ClientId"]).Value != null)
                    ClientId = ((JValue)obj["ClientId"]).Value.ToString();


                if (obj["UserRights"] != null && obj["UserRights"].HasValues)
                {              
                    JArray userRightsObj = JArray.FromObject(obj["UserRights"]);

                    if (userRightsObj != null)
                    {
                        UserRights.ReadJson(obj);
                    }
                }

                ObjectState = ObjectStates.None;
                SetOriginal();
            }
            catch (Exception ex)
            {

            }
        }


        public override void Cancel()
        {

        }

        public override void Update()
        {

        }



    }

}
