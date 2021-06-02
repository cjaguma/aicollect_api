using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(SubSectionConverter))]
    public class SubSection : DataCollectionObject
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
        public string QuestionaireKey { get; set; }
        [DataMember]
        public string SectionKey { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [Browsable(false)]
        public new SubSections Parent
        {
            get
            {
                return base.Parent as SubSections;
            }
        }

        public SubSection(AiCollectObject parent) : base(parent)
        {
            Questions = new Questions(this);
        }
        private SubSection _original;
        internal override void SetOriginal()
        {
            _original = Copy() as SubSection;
        }
        public override DatabaseQueries CreateTable(DataProviders provider, string tableScript)
        {
            return base.CreateTable(provider, tableScript);
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

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
            {
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());
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
                        question.ReadJson(cobj);
                    }
                }
            }

            ObjectState = ObjectStates.None;
            SetOriginal();
        }

        public override void Cancel()
        {
            switch(ObjectState)
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

    }
}
