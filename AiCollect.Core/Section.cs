using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(SectionConverter))]
    public class Section : DataCollectionObject
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
        [Browsable(false)]
        public Questions Questions { get; set; }

        [DataMember]
        [Browsable(false)]
        public SubSections SubSections { get; set; }
        [DataMember]
        public bool Deleted { get; set; }

        [Browsable(false)]
        public new Sections Parent
        {
            get
            {
                return base.Parent as Sections;
            }
        }
        [DataMember]
        public bool IsCompleted { get; set; }

        [DataMember]
        public string QuestionaireKey { get; set; }
        [DataMember]
        public string CertificationKey { get; set; }
        [DataMember]
        public string InspectionKey { get; set; }

        [DataMember]
        public string TemplateKey { get; set; }

        //[DataMember]
        //public SkipConditions Conditions { get; private set; }
        [DataMember]
        public string Description { get; set; }

        public Section(AiCollectObject parent) : base(parent)
        {
            Questions = new Questions(this);
            SubSections = new SubSections(this);
            //Conditions = new SkipConditions(this);
        }

        private Section _original;
        internal override void SetOriginal()
        {
            _original = Copy() as Section;
        }

        public override DatabaseQueries CreateTable(DataProviders provider, string tableScript)
        {
            return base.CreateTable(provider, tableScript);
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

        public override void Cancel()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                    Parent.Remove(this);
                    break;
                case ObjectStates.Modified:
                    break;
            }
        }

        public override void Update()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    Validate();
                    break;
                case ObjectStates.Removed:
                    break;
            }
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, bool checkForeignKeys, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, checkForeignKeys, provider);
        }

        public override DatabaseQueries GenerateDatabase(DataProviders provider)
        {
            return base.GenerateDatabase(provider);
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
            {
                Name = ((JValue)obj["Name"]).Value.ToString();
            }
            if (obj["Description"] != null && ((JValue)obj["Description"]).Value != null)
            {
                Description = ((JValue)obj["Description"]).Value.ToString();
            }
            if (obj["QuestionaireKey"] != null && ((JValue)obj["QuestionaireKey"]).Value != null)
            {
                QuestionaireKey = ((JValue)obj["QuestionaireKey"]).Value.ToString();
            }
            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
            {
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());
            }

            if (obj["SubSections"] != null)
            {
                JArray subSectionsObjs = JArray.FromObject(obj["SubSections"]);
                if (subSectionsObjs != null)
                {
                    this.SubSections.Clear();
                    foreach (JObject cobj in subSectionsObjs)
                    {
                        SubSection subSection = SubSections.Add();
                        subSection.ReadJson(cobj);
                    }
                }
            }

            if (obj["Questions"] != null)
            {
                JArray questionsObjs = JArray.FromObject(obj["Questions"]);
                if (questionsObjs != null)
                {
                    this.Questions.Clear();
                    foreach (JObject cobj in questionsObjs)
                    {
                        QuestionTypes questionType = QuestionTypes.None;
                        if (cobj["QuestionType"] != null && ((JValue)cobj["QuestionType"]).Value != null)
                        {
                            questionType = (QuestionTypes)Enum.Parse(typeof(QuestionTypes), ((JValue)cobj["QuestionType"]).Value.ToString());
                        }
                        Question question = Questions.Add(questionType);
                        if (question != null)
                            question.ReadJson(cobj);
                    }
                }
            }

            ObjectState = ObjectStates.None;
            SetOriginal();
        }

        public int Compare(Section other)
        {
            var chkName = this.Name == other.Name;


            return 1;
        }

        public DatabaseQueries Insert()
        {
            DatabaseQueries dbQueries = new DatabaseQueries();

            DatabaseQuery dbQuery = new DatabaseQuery();

            dbQuery.FriendlyMessage = "inserting into dsto_section";

            string query = string.Empty;

            query += string.Format("INSERT INTO dsto_questionaire (guid,name,yref_questionaire) values('{0}','{1}')", Key, Name, Parent.Parent.Key);
            dbQuery.SqlStatement = query;
            dbQuery.Name = TableName;
            dbQuery.Message = Messages.InsertRecord;
            dbQueries.Add(dbQuery);


            return dbQueries;
        }

    }
}
