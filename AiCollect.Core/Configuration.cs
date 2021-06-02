using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(ConfigurationConverter))]
    public class Configuration : AiCollectObject, IComparable<Configuration>, IUserParent
    {
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

        protected string _fileName;
        [DataMember]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        [DataMember]
        public bool Deleted { get; set; }
        private string _icon;

        [DataMember]
        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (value != _icon)
                {
                    _icon = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }


        //NOTE: Sync direction must e enforced
        private SyncDirection _syncDirection;

        [DataMember]
        public SyncDirection SyncDirection
        {
            get
            {
                return _syncDirection;
            }
            set
            {
                if (value != _syncDirection)
                {
                    _syncDirection = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }

        [DataMember]
        public DateTime CreatedOn { get; set; }



        [DataMember]
        public DateTime ModifiedOn { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        public DateTime Date
        {
            get
            {
                if (ModifiedOn == null)
                    return CreatedOn;
                else
                    return ModifiedOn;
            }
        }

        protected bool _requireLocation;

        [DataMember]
        public bool RequireLocation
        {
            get { return _requireLocation; }
            set
            {
                if (value != _requireLocation)
                {
                    _requireLocation = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        protected string _name;

        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        protected string _domain;
        /// <summary>
        /// Gets or Sets the domain of this project configuration to be used with authentication
        /// </summary>
        [DataMember]
        public string Domain
        {
            get { return _domain; }
            set
            {
                if (value != _domain)
                {
                    _domain = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        protected string _database;

        [DataMember]
        public string Database
        {
            //get { return _database; }
            set
            {
                if (value != _database)
                {
                    _database = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
            get
            {
                return Key;
            }
        }

        public string ConfigurationString { get; set; }
        protected string _description;

        [DataMember]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }


        public ConfigurationVersion ConfigVersion { get; private set; }


        [DataMember]
        public string Version
        {
            get
            {
                return ConfigVersion.ToString();
            }
            set
            {
                ConfigVersion.VersionString = value;
            }
        }

        protected int _status;

        [DataMember]
        public int Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public Questionaires Questionaires { get; set; }
        [DataMember]
        public EnumLists EnumerationLists { get; set; }
        [DataMember]
        public Users Users { get; set; }
        [DataMember]
        public UserRights UserRights { get; private set; }


        [DataMember]
        public Devices Devices { get; private set; }



        [DataMember]
        public DbManager DbInfo { get; private set; }

        [DataMember]
        public Settings Settings { get; private set; }
        [DataMember]
        public Purchases Purchases { get; private set; }
        [DataMember]
        public Certifications Certifications { get; set; }
        [DataMember]
        public Trainings Trainings { get; set; }
        [DataMember]
        public EnumList Regions { get; set; }
        [DataMember]
        public EnumList Products { get; set; }
        [DataMember]
        public EnumList Prices { get; set; }
        [DataMember]
        public FieldInspections Inspections { get; set; }
        [DataMember]
        public Client Client { get; set; }
        [DataMember]
        public Languages Languages { get; set; }

        [DataMember]
        public ConfigurationTypes Type { get; set; }

        [DataMember]
        public Translatables Translatables { get; private set; }

        public Modules Modules { get; private set; }
        public Configuration() : base(null)
        {
            Init();
        }


        private void Init()
        {
            Client = new Client(this);
            Settings = new Settings();
            Questionaires = new Questionaires(this);
            EnumerationLists = new EnumLists(this);
            Users = new Users(this);
            UserRights = new UserRights(this);
            Devices = new Devices(this);
            DbInfo = new DbManager(this);
            ConfigVersion = new ConfigurationVersion(this);
            Purchases = new Purchases(this);
            Certifications = new Certifications(this);
            Trainings = new Trainings(this);
            Languages = new Languages(this);
            Translatables = new Translatables();
            InitRegions();
            InitProducts();
            InitLanguages();
            Inspections = new FieldInspections();
            Modules = new Modules();
        }

        public void InitUserRights()
        {
            UserRight userRight = UserRights.Add();
            userRight.ObjectName = "Certification";
            userRight.ObjectType = ObjectType.Certification;

            userRight = UserRights.Add();
            userRight.ObjectName = "Admin";
            userRight.ObjectType = ObjectType.Admin;

            userRight = UserRights.Add();
            userRight.ObjectName = "Configuration";
            userRight.ObjectType = ObjectType.Configuration;

            userRight = UserRights.Add();
            userRight.ObjectName = "Inspection";
            userRight.ObjectType = ObjectType.EnumList;

            userRight = UserRights.Add();
            userRight.ObjectName = "Purchase";
            userRight.ObjectType = ObjectType.Purchase;

            userRight = UserRights.Add();
            userRight.ObjectName = "Training";
            userRight.ObjectType = ObjectType.Training;

            userRight = UserRights.Add();
            userRight.ObjectName = "SuperAdmin";
            userRight.ObjectType = ObjectType.SuperAdmin;

            InitQuestionaireUserRights();
            InitEnumListsUserRights();
            InitCertificationsUserRights();
            InitTrainingsUserRights();
            InitInspectionUserRights();
        }

        private void InitInspectionUserRights()
        {
            foreach(var inspection in Inspections)
            {
                var userRight = UserRights.Add();
                userRight.ObjectName = inspection.FieldName;
                userRight.ObjectType = ObjectType.Inspection;

                foreach(var section in inspection.Sections)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectName = section.Name;
                    userRight.ObjectType = ObjectType.Section;
                    foreach (var subSection in section.SubSections)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = subSection.Name;
                        userRight.ObjectType = ObjectType.Subsection;
                        foreach (var qn in subSection.Questions)
                        {
                            userRight = UserRights.Add();
                            userRight.ObjectName = qn.Name;
                            userRight.ObjectType = ObjectType.Question;
                        }
                    }
                    foreach (var qn in section.Questions)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = qn.Name;
                        userRight.ObjectType = ObjectType.Question;
                    }
                }
            }
        }

        private void InitQuestionaireUserRights()
        {
            foreach (var qnaire in Questionaires)
            {
                UserRight userRight = UserRights.Add();
                userRight.ObjectName = qnaire.Name;
                userRight.ObjectType = ObjectType.Questionaire;

                foreach (var section in qnaire.Sections)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectName = section.Name;
                    userRight.ObjectType = ObjectType.Section;
                    foreach (var subSection in section.SubSections)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = subSection.Name;
                        userRight.ObjectType = ObjectType.Subsection;
                        foreach (var qn in subSection.Questions)
                        {
                            userRight = UserRights.Add();
                            userRight.ObjectName = qn.Name;
                            userRight.ObjectType = ObjectType.Question;
                        }
                    }
                    foreach (var qn in section.Questions)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = qn.Name;
                        userRight.ObjectType = ObjectType.Question;
                    }
                }
            }
        }

        private void InitEnumListsUserRights()
        {
            foreach (var enumList in EnumerationLists)
            {
                UserRight userRight = UserRights.Add();
                userRight.ObjectName = enumList.Name;
                userRight.ObjectType = ObjectType.EnumList;
            }
        }

        private void InitCertificationsUserRights()
        {
            foreach(var certification in Certifications)
            {
                UserRight userRight = UserRights.Add();
                userRight.ObjectName = certification.Name;
                userRight.ObjectType = ObjectType.Certification;
                foreach (var section in certification.Sections)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectName = section.Name;
                    userRight.ObjectType = ObjectType.Section;
                    foreach (var subSection in section.SubSections)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = subSection.Name;
                        userRight.ObjectType = ObjectType.Subsection;
                        foreach (var qn in subSection.Questions)
                        {
                            userRight = UserRights.Add();
                            userRight.ObjectName = qn.Name;
                            userRight.ObjectType = ObjectType.Question;
                        }
                    }
                    foreach (var qn in section.Questions)
                    {
                        userRight = UserRights.Add();
                        userRight.ObjectName = qn.Name;
                        userRight.ObjectType = ObjectType.Question;
                    }
                }
            }          
        }

        private void InitTrainingsUserRights()
        {
            foreach (var training in Trainings)
            {
                UserRight userRight = UserRights.Add();
                userRight.ObjectName = training.Name;
                userRight.ObjectType = ObjectType.Training;
                foreach(var trainee in training.Trainees)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectType = ObjectType.Training;
                }
                foreach (var trainer in training.Trainers)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectName = trainer.Name;
                    userRight.ObjectType = ObjectType.Training;
                }
                foreach (var topic in training.Topics)
                {
                    userRight = UserRights.Add();
                    userRight.ObjectName = topic.Name;
                    userRight.ObjectType = ObjectType.Training;
                }
            }
        }

        private void InitProducts()
        {
            var exists = EnumerationLists.ByName("Products") != null;
            if (!exists)
            {
                Products = EnumerationLists.Add();
                Products.Name = "Products";
                EnumListValue enumListValue = Products.EnumValues.Add();
                enumListValue.Code = 1;
                enumListValue.Description = "Coffee";
                enumListValue = Products.EnumValues.Add();
                enumListValue.Code = 2;
                enumListValue.Description = "Cotton";
                enumListValue = Products.EnumValues.Add();
                enumListValue.Code = 3;
                enumListValue.Description = "Beans";
                enumListValue = Products.EnumValues.Add();
                enumListValue.Code = 4;
                enumListValue.Description = "Vanilla";
            }
        }

        private void InitLanguages()
        {
            Language language = Languages.Add();
            language.Name = "English";
            language.Code = "en";
            language.IsDefault = true;

            language = Languages.Add();
            language.Code = "Fr";
            language.Name = "French";

            language = Languages.Add();
            language.Code = "sw";
            language.Name = "Kiswahili";

        }

        private void InitRegions()
        {
            Regions = EnumerationLists.Add();
            Regions.Name = "Regions";
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 1, Description = "Buikwe" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 2, Description = "Bukomansimbi" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 3, Description = "Butambala" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 4, Description = "Buvuma" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 5, Description = "Gomba" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 6, Description = "Kalangala" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 7, Description = "Kalungu" });


            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 8, Description = "Kampala" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 9, Description = "Kayunga" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 10, Description = "Kiboga" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 11, Description = "Kyankwanzi" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 12, Description = "Luweero" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 13, Description = "Lwengo" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 14, Description = "Lyantonde" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 15, Description = "Masaka" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 16, Description = "Mityana" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 17, Description = "Mpigi" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 18, Description = "Mubende" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 19, Description = "Mukono" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 20, Description = "Nakaseke" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 21, Description = "Nakasongola" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 22, Description = "Rakai" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 23, Description = "Sembabule" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 24, Description = "Wakiso" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 25, Description = "Amuria" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 26, Description = "Budaka" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 27, Description = "Bududa" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 28, Description = "Bugiri" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 29, Description = "Bukedea" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 30, Description = "Bukwa" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 31, Description = "Bulambuli" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 32, Description = "Busia" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 33, Description = "Butaleja" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 34, Description = "Buyende" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 35, Description = "Iganga" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 36, Description = "Jinja" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 37, Description = "Kaberamaido" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 38, Description = "Kaliro" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 39, Description = "Kamuli" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 40, Description = "Kapchorwa" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 41, Description = "Katakwi" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 42, Description = "Kibuku" });

            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 43, Description = "Kumi" });
            Regions.EnumValues.Add(new EnumListValue(Regions.EnumValues) { Code = 44, Description = "Kween" });
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

        public override void ReadJson(JObject obj)
        {

            base.ReadJson(obj);

            CreatedOn = DateTime.Parse(((JValue)obj["CreatedOn"]).Value.ToString());

            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["ModifiedOn"] != null && ((JValue)obj["ModifiedOn"]).Value != null)
                ModifiedOn = DateTime.Parse(((JValue)obj["ModifiedOn"]).Value.ToString());

            if (obj["ModifiedBy"] != null && ((JValue)obj["ModifiedBy"]).Value != null)
                ModifiedBy = ((JValue)obj["ModifiedBy"]).Value.ToString();

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();

            if (obj["Domain"] != null && ((JValue)obj["Domain"]).Value != null)
                Domain = ((JValue)obj["Domain"]).Value.ToString();

            if (obj["Icon"] != null && ((JValue)obj["Icon"]).Value != null)
                Icon = ((JValue)obj["Icon"]).Value.ToString();
            if (obj["RequireLocation"] != null && ((JValue)obj["RequireLocation"]).Value != null)
                RequireLocation = Convert.ToBoolean(((JValue)obj["RequireLocation"]).Value);

            if (obj["Type"] != null && ((JValue)obj["Type"]).Value != null)
                Type = (ConfigurationTypes)Enum.Parse(typeof(ConfigurationTypes), ((JValue)obj["Type"]).Value.ToString());

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

            if (obj["Description"] != null && ((JValue)obj["Description"]).Value != null)
                Description = ((JValue)obj["Description"]).Value.ToString();

            if (obj["Status"] != null && ((JValue)obj["Status"]).Value != null)
                Status = Convert.ToInt32(((JValue)obj["Status"]).Value);

            if (obj["SyncDirection"] != null && ((JValue)obj["SyncDirection"]).Value != null)
                SyncDirection = (SyncDirection)Enum.Parse(typeof(SyncDirection), ((JValue)obj["SyncDirection"]).Value.ToString());

            //Configuration version information
            if (obj["ConfigVersion"] != null)
            {
                JObject verObj = JObject.FromObject(obj["ConfigVersion"]);
                if (verObj != null)
                    ConfigVersion.ReadJson(verObj);
            }

            //Questionaires
            if (obj["Questionaires"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Questionaires"]);
                if (dataListObj != null)
                    Questionaires.ReadJson(obj);
            }

            if (obj["Inspections"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Inspections"]);
                if (dataListObj != null)
                    Inspections.ReadJson(obj);
            }

            if (obj["Certifications"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Certifications"]);
                if (dataListObj != null)
                    Certifications.ReadJson(obj);
            }

            if (obj["Purchases"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Purchases"]);
                if (dataListObj != null)
                    Purchases.ReadJson(obj);
            }

            if (obj["Trainings"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Trainings"]);
                if (dataListObj != null)
                    Trainings.ReadJson(obj);
            }

            //Client
            if (obj["Client"] != null)
            {
                var clientObj = JObject.FromObject(obj["Client"]);
                if (clientObj != null)
                    Client.ReadJson(clientObj);
            }

            //Enum Lists
            if (obj["EnumerationLists"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["EnumerationLists"]);
                if (dataListObj != null)
                    EnumerationLists.ReadJson(obj);
            }

            //Users
            if (obj["Users"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Users"]);
                if (dataListObj != null)
                    Users.ReadJson(obj);
            }

            //UserRights
            if (obj["UserRights"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["UserRights"]);
                if (dataListObj != null)
                    UserRights.ReadJson(obj);
            }


            //Devices
            if (obj["Devices"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Devices"]);
                if (dataListObj != null)
                    Devices.ReadJson(obj);
            }

            //Modules
            if (obj["Modules"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Modules"]);
                if (dataListObj != null)
                    Modules.ReadJson(obj);
            }


        }
        /// <summary>
        /// Loads json configuration
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Configuration LoadJson(string json, JsonSerializerSettings settings)
        {
            Configuration configuration = JsonConvert.DeserializeObject<Configuration>(json, settings);
            return configuration;
        }
        public string JsonString
        {
            get; set;
        }
        public static Configuration OpenFile(string fileName)
        {
            string json = File.ReadAllText(fileName);
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.TypeNameHandling = TypeNameHandling.Auto;
            jsonSettings.Converters.Add(new ConfigurationConverter());
            Configuration configuration = JsonConvert.DeserializeObject<Configuration>(json, jsonSettings);
            configuration.FileName = fileName;
            JObject jobj = JObject.Parse(json);
            return configuration;
        }


        public bool Save()
        {
            try
            {
                using (FileStream fs = File.Open(FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, this);
                }
            }
            catch { return false; }
            return true;
        }

        private void GenerateDatabase()
        {
            //check database and fetch the configuration 
            // exists = DbInfo.Exists(); 
        }

        public override int CompareTo(AiCollectObject other)
        {

            Configuration configuration = this;

            Configuration importedConfig = other as Configuration;

            var checkCountCollectionObjects = configuration.Questionaires.Count == importedConfig.Questionaires.Count;
            //EnumerationLists
            var checkCountEnums = configuration.EnumerationLists.Count == importedConfig.EnumerationLists.Count;
            //UserRights
            var checkCountUserRights = configuration.UserRights.Count == importedConfig.UserRights.Count;
            //this.Users
            var checkCountUsers = configuration.Users.Count == importedConfig.Users.Count;

            var chkCounts = checkCountCollectionObjects && checkCountEnums && checkCountUserRights && checkCountUsers;

            var allSimilar = checkCountCollectionObjects && checkCountEnums && checkCountUserRights && checkCountUsers && chkCounts;

            return allSimilar ? 1 : 0;

        }

        public override JObject ToJson()
        {
            JObject configObj = JObject.FromObject(this);
            return configObj;
        }

        public string ToJsonString()
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.TypeNameHandling = TypeNameHandling.Auto;
            jsonSettings.Converters.Add(new ConfigurationConverter());
            string content = JsonConvert.SerializeObject(this);


            return content;
        }

        public DatabaseQueries CreateNewDatabase(DataProviders provider)
        {
            DatabaseQuery dbQuery = null;
            DatabaseQueries dbQueries = new DatabaseQueries();

            switch (provider)
            {
                case DataProviders.SQL:

                    string strCreateDbSQL = string.Format("CREATE DATABASE {0}", Key.AddSquareBrackets());
                    dbQuery = new DatabaseQuery();
                    dbQuery.SqlStatement = strCreateDbSQL;
                    dbQuery.Name = Database;
                    dbQuery.Message = Messages.CreateDatabase;
                    dbQueries.Add(dbQuery);

                    dbQuery = new DatabaseQuery();
                    strCreateDbSQL = string.Format("Use {0}", Key.AddSquareBrackets()); //Added for the export to file
                    dbQuery.SqlStatement = strCreateDbSQL;
                    dbQuery.Name = Database;
                    dbQuery.Message = Messages.UseDatabase;
                    dbQueries.Add(dbQuery);
                    break;

                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    dbQuery = new DatabaseQuery();
                    dbQuery.SqlStatement = string.Format("CREATE DATABASE {0}", Key.AddBackTicks());
                    dbQuery.Name = Database;
                    dbQuery.Message = Messages.CreateDatabase;
                    dbQueries.Add(dbQuery);


                    break;
            }
            if (dbQuery != null)
                dbQuery.FriendlyMessage = "Creating database " + Key;

            return dbQueries;
        }

        public DatabaseQueries DropDb(DataProviders provider)
        {
            DatabaseQueries dbQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = null;
            switch (provider)
            {
                case DataProviders.SQL:
                    dbQuery = new DatabaseQuery();
                    dbQuery.SqlStatement = string.Format("DROP DATABASE [{0}]", Database);
                    dbQuery.Name = Database;
                    dbQuery.Message = Messages.DropDatabase;
                    dbQueries.Add(dbQuery);
                    break;
                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    dbQuery = new DatabaseQuery();
                    dbQuery.SqlStatement = string.Format("DROP DATABASE {0}", Database.AddBackTicks());
                    dbQuery.Name = Database;
                    dbQuery.Message = Messages.DropDatabase;
                    dbQueries.Add(dbQuery);
                    break;
            }

            if (dbQuery != null)
                dbQuery.FriendlyMessage = "Dropping database " + Database;
            return dbQueries;
        }


        public DatabaseQueries GenerateJunctionTables()
        {
            DataProviders provider = DbInfo.DatabaseType;
            DatabaseQueries dbQueries = new DatabaseQueries();

            var scqueries = this.GenerateClosedQuestionsJunctionTable(provider);
            if (scqueries != null)
                dbQueries.Append(scqueries);

            scqueries = this.GenerateSectionsJunctionTable(provider);
            if (scqueries != null)
                dbQueries.Append(scqueries);

            scqueries = this.GenerateSubSectionsJunctionTable(provider);
            if (scqueries != null)
                dbQueries.Append(scqueries);

            return dbQueries;

        }

        /// <summary>
        /// Create the datalabs system table objects (dsto)
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public DatabaseQueries CreateSystemTables(DataProviders provider)
        {
            DatabaseQuery dbQuery;
            DatabaseQueries dbQueries = new DatabaseQueries();

            switch (provider)
            {
                case DataProviders.SQL:
                    //Configuration table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_configuration";
                    dbQuery.FriendlyMessage = "Creating system configuration table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"
					IF OBJECT_ID('dsto_configuration') IS NULL BEGIN CREATE TABLE [dsto_configuration] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
						[version] nvarchar(20) NOT NULL,
						[config] varchar(max) NOT NULL,                  
                        [status] int DEFAULT '0',
                        [created_on] datetime DEFAULT getdate(),
					) END
					";
                    dbQueries.Add(dbQuery);

                    //synchronisation conflict notifications
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_conflicts";
                    dbQuery.FriendlyMessage = "Creating system conflicts table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"
			IF OBJECT_ID('dsto_conflicts') IS NULL
                BEGIN
                    CREATE TABLE [dsto_conflicts] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
                        [created_on] datetime DEFAULT getdate(),
                        [created_by] nvarchar(25),
                        [last_updated_on] datetime DEFAULT getdate(),
                        [last_modified_by] nvarchar(25),
                        [imei] nvarchar(20) NOT NULL,
                        [data] varchar(MAX) NOT NULL,
                        [conflict_type] int DEFAULT '0',
                        [master_table_key] varchar(50) NOT NULL,
                        [master_row_oid] int NOT NULL,
                        [master_row_key] varchar(50) NOT NULL,
                        [status] int DEFAULT '0'
					)
                END
					";
                    dbQueries.Add(dbQuery);
                    //Create permissions table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_permissions";
                    dbQuery.FriendlyMessage = "Creating system permissions table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"
					CREATE TABLE [dsto_permissions] (
                        [OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
						[object_id] NVARCHAR(38) NOT NULL,
						[objectname] NVARCHAR(50) NOT NULL,
                        [permission] NTEXT NOT NULL,
                        [permission_type] INT DEFAULT '1'
					)
					";
                    dbQueries.Add(dbQuery);

                    //questions table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_questionaire";
                    dbQuery.FriendlyMessage = "Creating dsto_questionaire table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"			
                    CREATE TABLE [dsto_questionaire] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
                        [created_on] datetime DEFAULT getdate(),
                        [created_by] nvarchar(25),
                        [last_updated_on] datetime DEFAULT getdate(),
                        [last_modified_by] nvarchar(25),
                        [name] nvarchar(25)
					)           
					";
                    dbQueries.Add(dbQuery);

                    //sections table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_sections";
                    dbQuery.FriendlyMessage = "Creating sections table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"			
                    CREATE TABLE [dsto_sections] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
                        [created_on] datetime DEFAULT getdate(),
                        [created_by] nvarchar(25),
                        [last_updated_on] datetime DEFAULT getdate(),
                        [last_modified_by] nvarchar(25),
                        [questiontext] varchar(max) NOT NULL,
                        [question_type] int DEFAULT '0',
                        [answer] varchar(max) NOT NULL,
                        [yref_questionaire] uniqueidentifier references [dsto_questionaire]([guid])
					)           
					";
                    dbQueries.Add(dbQuery);

                    //subsections table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_sections";
                    dbQuery.FriendlyMessage = "Creating sections table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"			
                    CREATE TABLE [dsto_subsections] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
                        [created_on] datetime DEFAULT getdate(),
                        [created_by] nvarchar(25),
                        [last_updated_on] datetime DEFAULT getdate(),
                        [last_modified_by] nvarchar(25),
                        [questiontext] varchar(max) NOT NULL,
                        [question_type] int DEFAULT '0',
                        [answer] varchar(max) NOT NULL,
                        [yref_questionaire] uniqueidentifier references [dsto_questionaire]([guid]),
                        [yref_section] uniqueidentifier references [dsto_sections]([guid])
					)           
					";
                    dbQueries.Add(dbQuery);

                    //questions table
                    dbQuery = new DatabaseQuery();
                    dbQuery.Name = "dsto_questions";
                    dbQuery.FriendlyMessage = "Creating questions table";
                    dbQuery.Message = Messages.CreateTable;
                    dbQuery.SqlStatement = @"			
                    CREATE TABLE [dsto_questions] (
						[OID] int IDENTITY(1,1) NOT NULL,
                        [guid] uniqueidentifier default newid() PRIMARY KEY,
                        [created_on] datetime DEFAULT getdate(),
                        [created_by] nvarchar(25),
                        [last_updated_on] datetime DEFAULT getdate(),
                        [last_modified_by] nvarchar(25),
                        [questiontext] varchar(max) NOT NULL,
                        [question_type] int DEFAULT '0',
                        [answer] varchar(max) NOT NULL,
                        [yref_questionaire] uniqueidentifier references [dsto_questionaire]([guid]),
                        [yref_section] uniqueidentifier references [dsto_sections]([guid]),
                        [yref_subsection] uniqueidentifier references [dsto_subsections]([guid])
					)           
					";
                    dbQueries.Add(dbQuery);
                    break;
            }
            return dbQueries;
        }

        internal DatabaseQueries GenerateSectionsJunctionTable(DataProviders provider)
        {
            DatabaseQueries databaseQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_questions_sections";
            dbQuery.FriendlyMessage = "Creating questionsxsections junction table";
            dbQuery.Message = Messages.CreateTable;
            string query = string.Empty;
            switch (provider)
            {
                case DataProviders.SQL:
                    query = "create table dsto_sectionXquestion";
                    query += "(";
                    query += "oid int identity(1,1) not null,";
                    query += "[guid] uniqueidentifier default newid() primary key,";
                    query += "created_by varchar(50),";
                    query += "created_on datetime default getdate(),";
                    query += "deleted int default 0,";
                    query += "yref_question uniqueidentifier references dsto_questions([guid]),";
                    query += "yref_section uniqueidentifier references dsto_sections([guid])";
                    query += ")";
                    break;

            }

            dbQuery.SqlStatement = query;
            databaseQueries.Add(dbQuery);
            return databaseQueries;

        }

        internal DatabaseQueries GenerateSubSectionsJunctionTable(DataProviders provider)
        {
            DatabaseQueries databaseQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_questions";
            dbQuery.FriendlyMessage = "Creating questions table";
            dbQuery.Message = Messages.CreateTable;
            string query = string.Empty;
            switch (provider)
            {
                case DataProviders.SQL:
                    query = "create table dsto_subsectionXquestion";
                    query += "(";
                    query += "oid int identity(1,1) not null,";
                    query += "[guid] uniqueidentifier default newid() primary key,";
                    query += "created_by varchar(50),";
                    query += "created_on datetime default getdate(),";
                    query += "deleted int default 0,";
                    query += "yref_question uniqueidentifier references dsto_questions([guid]),";
                    query += "tref_section uniqueidentifier references dsto_subsections([guid])";
                    query += ")";
                    break;
            }
            dbQuery.SqlStatement = query;
            databaseQueries.Add(dbQuery);
            return databaseQueries;
        }


        internal DatabaseQueries GenerateClosedQuestionsJunctionTable(DataProviders provider)
        {

            List<EnumList> answerLists = new List<EnumList>();

            foreach (var questionaire in Questionaires)
            {

                if (!(questionaire is Questionaire)) continue;

                Questionaire qn = questionaire as Questionaire;
                foreach (Section section in qn.Sections)
                {
                    foreach (Question closedQuestion in section.Questions)
                    {
                        if (!(closedQuestion is ClosedQuestion)) continue;

                        if (!answerLists.Contains((closedQuestion as ClosedQuestion).EnumList))
                        {
                            answerLists.Add((closedQuestion as ClosedQuestion).EnumList);
                        }

                        foreach (SubSection sb in section.SubSections)
                        {
                            foreach (Question clq in sb.Questions)
                            {
                                if (!(closedQuestion is ClosedQuestion)) continue;

                                if (!answerLists.Contains((closedQuestion as ClosedQuestion).EnumList))
                                {
                                    answerLists.Add((closedQuestion as ClosedQuestion).EnumList);
                                }
                            }
                        }

                    }
                }
            }

            if (answerLists.Count == 0) return null;

            DatabaseQueries queries = new DatabaseQueries();

            foreach (var enumList in answerLists)
            {
                //create the table script
                DatabaseQuery query = new DatabaseQuery();
                query.Name = $"dsto_questionsX{ enumList.TableName}";
                query.FriendlyMessage = $"dsto_questionsX{ enumList.TableName}";
                query.Message = Messages.CreateTable;
                switch (provider)
                {
                    case DataProviders.SQL:
                        query.SqlStatement = $"create table dsto_questionsX{enumList.TableName}(";
                        query.SqlStatement += "oid int identity(1,1) not null,";
                        query.SqlStatement += "[guid] uniqueidentifier default newid() primary key,";
                        query.SqlStatement += "deleted int default 0,";
                        query.SqlStatement += "created_on datetime default getdate(),";
                        query.SqlStatement += "created_by varchar(50),";
                        query.SqlStatement += "last_updated_on datetime,";
                        query.SqlStatement += "last_modified_by varchar(50),";
                        query.SqlStatement += "yref_question uniqueidentifier references dsto_questions([guid]),";
                        query.SqlStatement += $"enum_oid int references {enumList.TableName}(oid)";
                        query.SqlStatement += ")";
                        break;

                }
                queries.Add(query);
            }
            return queries;
        }

        public DatabaseQueries CreateSaveConfiguration(DataProviders provider)
        {
            DatabaseQuery dbQuery;
            DatabaseQueries dbQueries;
            dbQueries = new DatabaseQueries();
            dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_configuration";
            dbQuery.FriendlyMessage = "Saving configuration to database";
            dbQuery.Message = Messages.InsertRecord;
            switch (provider)
            {
                case DataProviders.SQL:
                    dbQuery.SqlStatement = string.Format("INSERT INTO [dsto_configuration]([version],[config],[guid],[status]) VALUES('{0}','{1}','{2}','{3}')", ConfigVersion.ToString(), ToJsonString(), Key, Status);
                    break;
                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    dbQuery.SqlStatement = string.Format("INSERT INTO dsto_configuration(`version`,`config`,`guid`,`status`) VALUES('{0}','{1}','{2}','{3}')", ConfigVersion.ToString(), ToJsonString(), Key, Status);
                    break;
            }


            dbQueries.Add(dbQuery);

            return dbQueries;
        }

        public string SaveQuery
        {
            get
            {
                return string.Format("INSERT INTO [dsto_configuration]([version],[config],[guid],[status]) VALUES('{0}','{1}','{2}','{3}')", ConfigVersion.ToString(), ToJsonString(), Key, Status);
            }
        }

        public string GetStringQuery
        {
            get
            {
                return string.Format("SELECT [config] FROM [dsto_configuration] WHERE [guid] = '{0}'", Key);
            }
        }

        public bool IsParent => true;

        public DatabaseQueries CreateUpdateConfiguration(DataProviders provider)
        {
            DatabaseQuery dbQuery;
            DatabaseQueries dbQueries;
            dbQueries = new DatabaseQueries();
            dbQuery = new DatabaseQuery();
            dbQuery.Name = "dsto_configuration";
            dbQuery.FriendlyMessage = "Saving configuration to database";
            dbQuery.Message = Messages.InsertRecord;
            switch (provider)
            {
                case DataProviders.SQL:
                    dbQuery.SqlStatement = string.Format("UPDATE [dsto_configuration] SET [version]='{0}',[config]='{1}',[status]='{3}' WHERE [key]='{2}'", ConfigVersion.ToString(), ToJsonString(), Key, Status);
                    break;

                case DataProviders.SQLite:
                case DataProviders.MYSQL:
                    dbQuery.SqlStatement = string.Format("UPDATE dsto_configuration SET `version`='{0}',`config`='{1}',`status`='{3}'WHERE `key`='{2}'", ConfigVersion.ToString(), ToJsonString(), Key, Status);
                    break;
            }


            dbQueries.Add(dbQuery);

            return dbQueries;
        }

        public int CompareTo(Configuration other)
        {
            Configuration serverConfiguration = other;
            return 1;
        }


        #region Translate

        public string Translate(string text)
        {
            return Translatables.Translate(text, this);
        }



        #endregion

    }
}
