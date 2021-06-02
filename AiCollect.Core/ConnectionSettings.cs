using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class ConnectionSettings:AiCollectObject
    {
        private DataProviders _provider;
        [DataMember]
        public DataProviders Provider
        {
            get { return _provider; }
            set
            {
                if (value != _provider)
                {
                    _provider = value;
                  
                    ObjectState = ObjectStates.Modified;
                    switch (_provider)
                    {
                        case DataProviders.MYSQL:
                            Port = 3306;
                            break;
                        case DataProviders.SQL:
                            Port = 1433;
                            break;
                    }
                }

            }
        }


        private string _server;
        [DataMember]
        public string Server
        {
            get { return _server; }
            set
            {
                if (value != _server)
                {
                    _server = value;
                    ObjectState = ObjectStates.Modified;
                    
                }
            }
        }

        private string _database;
        [DataMember]
        public string Database
        {
            get { return _database; }
            set
            {
                if (value != _database)
                {
                    _database = value;
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

        private string _userName;
        [DataMember]
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    ObjectState = ObjectStates.Modified;
                   
                }
            }
        }

        private string _password;
        [DataMember]
        public string Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    ObjectState = ObjectStates.Modified;
                   
                }
            }
        }

        private int _port;
        [DataMember]
        public int Port
        {
            get { return _port; }
            set
            {
                if (value != _port)
                {
                    _port = value;
                    ObjectState = ObjectStates.Modified;
                    
                }
            }
        }

        private int _timeout;
        [DataMember]
        public int Timeout
        {
            get { return _timeout; }
            set
            {
                if (value != _timeout)
                {
                    _timeout = value;
                    ObjectState = ObjectStates.Modified;
                   
                }
            }
        }

        private string _path;
        [DataMember]
        public string Path
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(_path))
                    return string.Format(@"Cache\Stores\{0}.store", Configuration.Key);
                else
                    return _path;
            }
            set
            {
                if (value != _path)
                {
                    _path = value;
                    ObjectState = ObjectStates.Modified;
                   
                }
            }
        }

        public string FullPath { get; set; }
        public string ConnectionString
        {
            get
            {
                string connString = string.Empty;
                switch (Provider)
                {
                    case DataProviders.SQL:
                        if (Authentication == Authentications.Database)
                            connString = string.Format("Server={0};Database={1};User Id={2};Password={3};", Server, Database, UserName, Password);
                        else
                            connString = string.Format("Server={0};initial catalog={1};integrated security=true;", Server, Database);
                        break;
                    case DataProviders.MYSQL:
                        if (Authentication == Authentications.Database)
                        {
                            connString = string.Format("Server={0};Port={4};Database={1};Uid={2};Pwd={3};", Server, Database, UserName, Password, Port);
                        }
                        else
                        {
                            connString = string.Format("Server={0};Database={1};IntegratedSecurity=yes;Uid=auth_windows;", Server, Database);
                        }
                      
                        if (Timeout > 0)
                        {
                            connString += string.Format("Connection Timeout={1};",Timeout);
                        }
                        break;
                    case DataProviders.SQLite:
                        connString = string.Format("Data Source={0};Version=3;Password={1};", string.IsNullOrWhiteSpace(FullPath) ? Path : FullPath, Password);
                        break;

                    case DataProviders.SQLCE:
                        connString = string.Format("Data Source={0};Password={1};Persist Security Info=False;",string.IsNullOrWhiteSpace(FullPath)? Path:FullPath, Password);
                        break;
                }

                return connString;
            }
        }

        public string MasterConnectionString
        {
            get
            {
                string connString = string.Empty;
                switch (Provider)
                {
                    case DataProviders.SQL:
                        if (Authentication == Authentications.Database)
                            connString = string.Format("Server={0};Database=Master;User Id={1};Password={2};", Server, UserName, Password);
                        else
                            connString = string.Format("Server={0};Database=Master;Trusted_Connection=True;", Server);
                        break;
                    case DataProviders.MYSQL:
                        connString = string.Format("Server={0};Uid={1};Pwd={2};Port={3};", Server, UserName, Password, Port);
                        if (Timeout > 0)
                        {
                            connString += string.Format("Connection Timeout={1};", Timeout);
                        }
                        break;
                    case DataProviders.SQLite:
                        connString = string.Format("Data Source={0};Version=3;Password={1};", string.IsNullOrWhiteSpace(FullPath) ? Path : FullPath, Password);
                        break;

                 
                }

                return connString;
            }
        }

        public bool IsValid
        {
            get
            {
                bool isValid = true;

                switch (Provider)
                {
                    case DataProviders.SQLite:
                    case DataProviders.SQLCE:
                        if (string.IsNullOrWhiteSpace(Path))
                            isValid = false;
                        break;
                    case DataProviders.MYSQL:
                    case DataProviders.SQL:
                        if(string.IsNullOrWhiteSpace(Database))
                            isValid=false;
                        if (string.IsNullOrWhiteSpace(Server))
                            isValid = false;

                        if (Authentication == Authentications.Database)
                        {
                            if (string.IsNullOrWhiteSpace(UserName))
                                isValid = false;
                            if (string.IsNullOrWhiteSpace(Password))
                                isValid = false;
                        }
                        break;
                }

                return isValid;
            }
        }

        protected ConnectionSettings OriginalValues { get; private set; }

        public ConnectionSettings(AiCollectObject parent)
            : base(parent)
        {
            Provider = DataProviders.SQL;
            Authentication = Authentications.Database;
            Server = "";
            Database = "";
            UserName = "";
            Password = "";
            Path = "";
            Port = 0;
            Timeout = 0;

            SetOriginalValues();
        }


        protected void SetOriginalValues()
        {
            OriginalValues = (ConnectionSettings)Copy();
            ObjectState = ObjectStates.Modified;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            Provider = (DataProviders)Enum.Parse(typeof(DataProviders), ((JValue)obj["Provider"]).Value.ToString());
            Server = ((JValue)obj["Provider"]).Value.ToString();
            Authentication = (Authentications)Enum.Parse(typeof(Authentications), ((JValue)obj["Authentication"]).Value.ToString());
            Database = ((JValue)obj["Database"]).Value.ToString();
            UserName = ((JValue)obj["UserName"]).Value.ToString();
            Password = ((JValue)obj["Password"]).Value.ToString();
            Path = ((JValue)obj["Path"]).Value.ToString();
            Port = int.Parse(((JValue)obj["Port"]).Value.ToString());
            Timeout = int.Parse(((JValue)obj["Timeout"]).Value.ToString());
        }

     

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:

                    break;
            }
        }

        public override void Cancel()
        {
            if(ObjectState != ObjectStates.None)
            {
                _provider = OriginalValues._provider;
                _server = OriginalValues._server;
                _userName = OriginalValues._userName;
                _password = OriginalValues._password;
                _port = OriginalValues._port;
                _path = OriginalValues._path;
                _authentication = OriginalValues._authentication;
                _timeout = OriginalValues._timeout;
                ObjectState = ObjectStates.None;
            }
        }

        public override void Update()
        {
            Validate();
            ObjectState = ObjectStates.None;
        }
    }
}
