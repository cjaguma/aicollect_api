using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class DbManager:AiCollectObject
    {

        private string _server;

        [DataMember]
        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                if (value != _server)
                {
                    _server = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }

        private Authentications _authentication;

        [DataMember]
        public Authentications Authentication
        {
            get
            {
                return _authentication;
            }
            set
            {
                if (value != _authentication)
                {
                    _authentication = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        private string _user;

        [DataMember]
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }

        private string _password;

        [DataMember]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        private string _database;
        [DataMember]
        public string Database
        {
            get
            {
                return Configuration.Key;
            }
            //set
            //{
            //    if (value != _database)
            //    {
            //        _database = value;
            //        ObjectState = ObjectStates.Modified;
            //    }
            //}
        }

        private DataProviders _databaseType;

        public DbManager(AiCollectObject parent) : base(parent)
        {
        }

        public DbManager():base(null)
        {

        }
        [DataMember]
        public DataProviders DatabaseType
        {
            get { return _databaseType; }
            set
            {
                if (value != _databaseType)
                {
                    _databaseType = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [IgnoreDataMember]
        public new Configuration Parent
        {
            get
            {
                return (Configuration)base.Parent;
            }
        }

        [DataMember]
        public string MasterConnectionString
        {
            get
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("Server=");
                sb.Append(Server);
                sb.Append(";Database=Master");
                if (Authentication == Authentications.Windows)
                {
                    sb.Append(";Trusted_Connection=True;");
                }
                else
                {
                    sb.Append(";User ID=");
                    sb.Append(User);
                    sb.Append(";Password=");
                    sb.Append(Password);
                    sb.Append(";");
                }
                //Pooling
                sb.Append("Pooling=false;");
                return sb.ToString();
            }
        }
        [DataMember]
        public string ConnectionString
        {
            get
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("Server=");
                sb.Append(Server);
                sb.Append(";Database=");
                sb.Append(Database);
                if (Authentication == Authentications.Windows)
                {
                    sb.Append(";Trusted_Connection=True;");
                }
                else
                {
                    sb.Append(";User ID=");
                    sb.Append(User);
                    sb.Append(";Password=");
                    sb.Append(Password);
                    sb.Append(";");
                }
                //Pooling
                sb.Append("Pooling=false;");
                //Set the connection timeout
                sb.Append("Connection Timeout=6000");
                return sb.ToString();
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

    }
}
