using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(QuestionaireConverter))]
    public class Questionaire : DataCollectionObject, IComparable<Questionaire>
    {
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
        public new string CreatedBy
        {
            get
            {
                return base.CreatedBy;
            }
            set
            {
                base.CreatedBy = value;
            }
        }

        [DataMember]
        public new DateTime CreatedOn
        {
            get
            {
                return base.CreatedOn;
            }
            set
            {
                base.CreatedOn = value;
            }
        }

        private double _lattitude;
        private double _longitude;

        private Statuses _status;
        private Sections _sections;

        public new Questionaires Parent
        {
            get
            {
                return base.Parent as Questionaires;
            }
        }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public double Latitude
        {
            get
            {
                return _lattitude;
            }
            set
            {
                if (_lattitude != value)
                {
                    _lattitude = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public Statuses Status
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
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public string ConfigurationId { get; set; }
        [DataMember]
        public int ModuleId { get; set; }

        public Coordinates Coordinates { get; set; }

        [DataMember]
        public Regions Regions { get; set; }

        [DataMember]
        public string Template { get; set; }

        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public Categories Categories { get; set; }

        [DataMember]
        public Category Category { get; set; }

        [DataMember]
        public Region Region { get; set; }

        [DataMember]
        public string QuestionaireCode { get; set; }

        public Questionaire(AiCollectObject parent) : base(parent)
        {
            Sections = new Sections(this);
            Coordinates = new Coordinates(this);
            Categories = new Categories();
            Category = new Category(this);
            Region = new Region(this);
            Regions = new Regions();
            AddSystemSection();
        }


        private Questionaire _original;
        internal override void SetOriginal()
        {
            _original = Copy() as Questionaire;
        }

        [DataMember]
        public Sections Sections { get => _sections; set => _sections = value; }

        private void AddSystemSection()
        {
            Section section = Sections.Add();
            section.Name = "Registration";
            OpenQuestion oq = section.Questions.AddOpenQuestion();
            oq.Name = "Name";
            oq.QuestionText = "Name";

            oq = section.Questions.AddOpenQuestion();
            oq.Name = "Address";
            oq.QuestionText = "Address";

            oq = section.Questions.AddOpenQuestion();
            oq.Name = "Contact";
            oq.QuestionText = "Contact";

        }


        public override void Cancel()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                    Parent.Remove(this);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    if (string.IsNullOrWhiteSpace(Name))
                        throw new Exception("Name cannot be empty");
                    break;
            }
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
            {
                Name = ((JValue)obj["Name"]).Value.ToString();
            }

            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
            {
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();
            }

            if (obj["Notes"] != null && ((JValue)obj["Notes"]).Value != null)
            {
                Notes = ((JValue)obj["Notes"]).Value.ToString();
            }

            if (obj["Latitude"] != null && ((JValue)obj["Latitude"]).Value != null)
            {
                Latitude = double.Parse(((JValue)obj["Latitude"]).Value.ToString());
            }

            if (obj["Longitude"] != null && ((JValue)obj["Longitude"]).Value != null)
            {
                Longitude = double.Parse(((JValue)obj["Longitude"]).Value.ToString());
            }

            if (obj["Status"] != null && ((JValue)obj["Status"]).Value != null)
            {
                Status = (Statuses)Enum.Parse(typeof(Statuses), ((JValue)obj["Status"]).Value.ToString());
            }

            if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
            {
                ConfigurationId = ((JValue)obj["ConfigurationId"]).Value.ToString();
            }
            
            if (obj["Code"] != null && ((JValue)obj["Code"]).Value != null)
            {
                Code = ((JValue)obj["Code"]).Value.ToString();
            }

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
            {
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());
            }

            if (obj["Template"] != null && ((JValue)obj["Template"]).Value != null)
            {
                Template = ((JValue)obj["Template"]).Value.ToString();
            }

            if (obj["Categories"] != null)
            {
                JArray categoriesObjs = JArray.FromObject(obj["Categories"]);
                Categories.Clear();
                Categories.ReadJson(categoriesObjs);
            }

            if (obj["Regions"] != null)
            {
                JArray regionsObjs = JArray.FromObject(obj["Regions"]);
                Regions.Clear();
                Regions.ReadJson(regionsObjs);
            }

            if (obj["Region"] != null && obj["Region"].HasValues)
            {
                var regionObj = JObject.FromObject(obj["Region"]);
                if (regionObj != null)
                    Region.ReadJson(regionObj);
            }

            if (obj["Category"] != null && obj["Category"].HasValues)
            {
                var categoryObj = JObject.FromObject(obj["Category"]);
                if (categoryObj != null)
                    Category.ReadJson(categoryObj);
            }

            _sections.Clear();
            _sections.ReadJson(obj);
            ObjectState = ObjectStates.None;
            SetOriginal();

        }

        public override DatabaseQueries CreateTable(DataProviders provider, string tableScript)
        {
            DatabaseQueries dbQueries = new DatabaseQueries();
            DatabaseQuery dbQuery = new DatabaseQuery();
            dbQuery.FriendlyMessage = "Creating table " + TableName;

            switch (provider)
            {
                case DataProviders.SQL:
                    tableScript += string.Format("IF OBJECT_ID('{0}') IS NULL\n", TableName);
                    tableScript += "BEGIN";
                    tableScript += string.Format("CREATE TABLE {0}(", TableName.AddSquareBrackets());

                    break;
                case DataProviders.MYSQL:
                case DataProviders.SQLite:
                    tableScript += string.Format("CREATE TABLE {0}(", TableName);
                    tableScript += " Name varchar(100) not null";
                    break;
                default:
                    tableScript += string.Format("CREATE TABLE {0}(", TableName);
                    tableScript += " Name varchar(100) not null";
                    break;
            }

            tableScript += " OID INT IDENTITY(1,1) PRIMARY KEY,";
            tableScript += " created_on datetime not null,";
            tableScript += " created_by varchar(100) not null,";
            tableScript += " last_updated_on datetime not null,";
            tableScript += " last_updated_by varchar(100) not null,";
            tableScript += " Name varchar(100) not null";

            tableScript += ")";

            if (provider == DataProviders.SQL)
                tableScript += "\nEND";
            dbQuery.SqlStatement = tableScript;
            dbQuery.Name = TableName;
            dbQuery.Message = Messages.CreateTable;
            dbQueries.Add(dbQuery);

            return dbQueries;
        }


        public DatabaseQueries Insert()
        {
            DatabaseQueries dbQueries = new DatabaseQueries();

            DatabaseQuery dbQuery = new DatabaseQuery();

            dbQuery.FriendlyMessage = "inserting into dsto_questionaire";

            string query = string.Empty;

            query += string.Format("INSERT INTO dsto_questionaire (guid,name) values('{0}','{1}')", Key, Name);
            dbQuery.SqlStatement = query;
            dbQuery.Name = TableName;
            dbQuery.Message = Messages.InsertRecord;
            dbQueries.Add(dbQuery);


            return dbQueries;
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, bool checkForeignKeys, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, checkForeignKeys, provider);
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, provider);
        }

        public override DatabaseQueries GenerateDatabase(DataProviders provider)
        {
            return base.GenerateDatabase(provider);
        }

        public int CompareTo(Questionaire other)
        {
            var chkName = this.Name == other.Name;
            var chkStatus = this.Status == other.Status;
            var chkLatitude = this.Latitude == other.Latitude;
            var chkLongitude = this.Longitude == other.Longitude;

            var chkSections = this.Sections.Count == other.Sections.Count;
            var coordinates = this.Coordinates.Count == other.Coordinates.Count;

            if(!chkSections)
            {
                foreach(var section in this.Sections)
                {

                }
            }

            return 1;
        }
    }
}
