using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(QuestionConverter))]
    public abstract class Question : DataCollectionObject
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
        public bool Deleted { get; set; }

        private QuestionTypes _questionType;

        private string _questionText;
        [DataMember]
        public string QuestionText
        {
            get
            {
                return _questionText;
            }
            set
            {
                if (_questionText != value)
                {
                    _questionText = value;
                    Name = _questionText;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public Answers Answers { get; set; }

        [DataMember]
        public bool Required { get; set; }

        [Browsable(false)]
        public new Questions Parent
        {
            get
            {
                return base.Parent as Questions;
            }
        }

        private DataTypes _dataType;

        [DataMember]
        public DataTypes DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                if (_dataType != value)
                {
                    _dataType = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public SkipConditions Conditions { get; set; }
        [DataMember]
        public Dependencies Dependencies { get; set; }

        public Question(AiCollectObject parent) : base(parent)
        {
            Conditions = new SkipConditions(this);
            Dependencies = new Dependencies(this);
            Answers = new Answers(this);
            QuestionText = string.Empty;
        }

        [DataMember]
        [Browsable(false)]
        public QuestionTypes QuestionType { get => _questionType; set => _questionType = value; }

        [DataMember]
        public string SectionKey { get; set; }

        [DataMember]
        public string SubSectionKey { get; set; }

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

            if (obj["QuestionText"] != null && ((JValue)obj["QuestionText"]).Value != null)
            {
                QuestionText = ((JValue)obj["QuestionText"]).Value.ToString();
            }

            if (obj["Required"] != null && ((JValue)obj["Required"]).Value != null)
            {
                //if(obj["Required"] != DBNull.Value)
                Required = bool.Parse(((JValue)obj["Required"]).Value.ToString());
            }

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
            {
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());
            }

            if (obj["DataType"] != null && ((JValue)obj["DataType"]).Value != null)
            {
                DataType = (DataTypes)Enum.Parse(typeof(DataTypes), ((JValue)obj["DataType"]).Value.ToString());
            }
            if (obj["Conditions"] != null)
            {
                JArray conditionsObjs = JArray.FromObject(obj["Conditions"]);
                if (conditionsObjs != null)
                {
                    Conditions.Clear();
                    foreach (var c in conditionsObjs)
                    {
                        SkipCondition skipCondition = Conditions.Add();
                        if (c["Key"] != null && ((JValue)c["Key"]).Value != null)
                        {
                            skipCondition.Key = ((JValue)c["Key"]).Value.ToString();
                        }
                        if (c["Qualifier"] != null && ((JValue)c["Qualifier"]).Value != null)
                        {
                            skipCondition.Qualifier = (Qualifiers)Enum.Parse(typeof(Qualifiers), ((JValue)c["Qualifier"]).Value.ToString());
                        }
                        if (c["AttributeKey"] != null && ((JValue)c["AttributeKey"]).Value != null)
                        {
                            skipCondition.AttributeKey = ((JValue)c["AttributeKey"]).Value.ToString();
                        }
                        if (c["DataCollectionObectType"] != null && ((JValue)c["DataCollectionObectType"]).Value != null)
                        {
                            skipCondition.DataCollectionObectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), ((JValue)c["DataCollectionObectType"]).Value.ToString());
                        }

                        if (c["Answer"] != null)
                        {
                            JObject answer = JObject.FromObject(c["Answer"]);
                            if (answer != null)
                            {
                                if (skipCondition.Answer == null)
                                    skipCondition.Answer = new EnumListValue(null);
                                skipCondition.Answer.ReadJson(answer);
                            }
                        }

                        if (c["Target"] != null)
                        {
                            var b = c["Target"] is JValue ? ((JValue)c["Target"]).Value == null : false;
                            if (!b)
                            {
                                JObject objTarget = JObject.FromObject(c["Target"]);
                                if (objTarget != null)
                                {
                                    switch (skipCondition.DataCollectionObectType)
                                    {
                                        case DataCollectionObectTypes.Section:
                                            skipCondition.Target.Section = new Sections().Add();
                                            skipCondition.Target.Section.ReadJson(JObject.FromObject(objTarget["Section"]));
                                            break;
                                        case DataCollectionObectTypes.SubSection:
                                            skipCondition.Target.SubSection = new SubSections().Add();
                                            skipCondition.Target.SubSection.ReadJson(JObject.FromObject(objTarget["SubSection"]));
                                            break;
                                        case DataCollectionObectTypes.Question:
                                            var qnObj = JObject.FromObject(objTarget["Question"]);
                                            skipCondition.Target.Question = new Questions().Add((QuestionTypes)Enum.Parse(typeof(QuestionTypes), ((JValue)qnObj["QuestionType"]).Value.ToString()));
                                            skipCondition.Target.Question.ReadJson(JObject.FromObject(qnObj));
                                            break;
                                    }
                                }
                            }

                        }

                    }
                }
            }


            if (obj["Dependencies"] != null)
            {
                JArray dependenciesObjs = JArray.FromObject(obj["Dependencies"]);
                if (dependenciesObjs != null)
                {
                    Dependencies = new Dependencies(null);
                    foreach (var d in dependenciesObjs)
                    {
                        Dependency dependency = Dependencies.Add();
                        if (d["Key"] != null && ((JValue)d["Key"]).Value != null)
                        {
                            dependency.Key = ((JValue)d["Key"]).Value.ToString();
                        }
                        if (d["QuestionKey"] != null && ((JValue)d["QuestionKey"]).Value != null)
                        {
                            dependency.QuestionKey = ((JValue)d["QuestionKey"]).Value.ToString();
                        }

                        if (d["TargetObjectKey"] != null && ((JValue)d["TargetObjectKey"]).Value != null)
                        {
                            dependency.TargetObjectKey = ((JValue)d["TargetObjectKey"]).Value.ToString();
                        }

                        if (d["TargetObjectKey"] != null && ((JValue)d["TargetObjectKey"]).Value != null)
                        {
                            dependency.TargetObjectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), ((JValue)d["TargetObjectType"]).Value.ToString());
                        }

                        if (d["Template"] != null && ((JValue)d["Template"]).Value != null)
                        {
                            dependency.Template = ((JValue)d["Template"]).Value.ToString();
                        }

                        if (d["Target"] != null)
                        {
                            var targetObj = (JObject)d["Target"];
                            dependency.Target = new Target();
                            switch (dependency.TargetObjectType)
                            {
                                case DataCollectionObectTypes.SubSection:
                                    dependency.Target.SubSection = new SubSections().Add();
                                    dependency.Target.SubSection.ReadJson((JObject)targetObj["SubSection"]);
                                    break;

                                case DataCollectionObectTypes.Section:
                                    dependency.Target.Section = new Sections().Add();
                                    dependency.Target.SubSection.ReadJson((JObject)targetObj["Section"]);
                                    break;
                            }
                        }
                    }
                }

            }

            if (obj["Answers"] != null)
            {
                JArray answersObjs = JArray.FromObject(obj["Answers"]);
                if (answersObjs != null)
                {
                    Answers.Clear();
                    foreach (var a in answersObjs)
                    {
                        if (a["AnswerText"] != null && ((JValue)a["AnswerText"]).Value != null)
                        {
                            Answer answer = new Answer(null);
                            if (a["Key"] != null && ((JValue)a["Key"]).Value != null)
                            {
                                answer.Key = ((JValue)a["Key"]).Value.ToString();
                            }

                            if (a["Occurance"] != null && ((JValue)a["Occurance"]).Value != null)
                            {
                                answer.Occurance = int.Parse(((JValue)a["Occurance"]).Value.ToString());
                            }

                            answer.AnswerText = ((JValue)a["AnswerText"]).Value.ToString();
                            Answers.Add(answer);
                        }
                    }

                }
                ObjectState = ObjectStates.None;
                SetOriginal();
            }
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

        public virtual int Compare(Question other)
        {
            var chkName = this.Name == other.Name;
            var chkQnText = this.QuestionText == other.QuestionText;

            return chkName && chkQnText ? 1 : 0;
        }
    }
}
