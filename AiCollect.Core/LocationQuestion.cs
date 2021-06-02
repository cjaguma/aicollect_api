using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class LocationQuestion : OpenQuestion
    {
        [DataMember]
        public new string Answer
        {
            get
            {
                return base.Answer;
            }
            set
            {
                base.Answer = value;
            }
        }

        public LocationQuestion(AiCollectObject parent) : base(parent)
        {
            QuestionType = QuestionTypes.Location;
        }

        public LocationQuestion():base(null)
        {
            QuestionType = QuestionTypes.Location;
        }

        public override void Cancel()
        {
            base.Cancel();
        }

        public override int Compare(Question other)
        {
            return base.Compare(other);
        }

        public override int CompareTo(AiCollectObject other)
        {
            return CompareTo(other);
        }

        public override DatabaseQueries CreateTable(DataProviders provider, string tableScript)
        {
            return base.CreateTable(provider, tableScript);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, provider);
        }

        public override DatabaseQueries GenerateDatabase(DataCollectionObject importedForm, bool checkForeignKeys, DataProviders provider)
        {
            return base.GenerateDatabase(importedForm, checkForeignKeys, provider);
        }

        public override DatabaseQueries GenerateDatabase(DataProviders provider)
        {
            return base.GenerateDatabase(provider);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Answer"] != null && ((JValue)obj["Answer"]).Value != null)
            {
                Answer = ((JValue)obj["Answer"]).Value.ToString();
            }
        }

        public override JObject ToJson()
        {
            return base.ToJson();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Validate()
        {
            base.Validate();
        }

        internal override void SetOriginal()
        {
            base.SetOriginal();
        }
    }
}
