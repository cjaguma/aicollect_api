using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class OpenQuestion : Question
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
        private string _answer;

        public string Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        private bool _isBinaryAnswer;

        public bool IsBinaryAnswer
        {
            get
            {
                return _isBinaryAnswer;
            }
            set
            {
                if (_isBinaryAnswer != value)
                {
                    _isBinaryAnswer = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public Dependency Dependency { get; set; }

        public OpenQuestion(AiCollectObject parent) : base(parent)
        {

        }

        private OpenQuestion _original;

        internal override void SetOriginal()
        {
            _original = Copy() as OpenQuestion;
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    if (string.IsNullOrWhiteSpace(QuestionText))
                        throw new Exception("Name cannot be empty");
                    break;
            }
        }

        public override void Cancel()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    Parent.Remove(this);
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

        public override void ReadJson(JObject obj)
        {

            base.ReadJson(obj);

            if (obj["QuestionText"] != null && ((JValue)obj["QuestionText"]).Value != null)
            {
                QuestionText = ((JValue)obj["QuestionText"]).Value.ToString();
            }

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
            {
                Name = ((JValue)obj["Name"]).Value.ToString();
            }

            if (obj["IsBinaryAnswer"] != null && ((JValue)obj["IsBinaryAnswer"]).Value != null)
            {
                IsBinaryAnswer = bool.Parse(((JValue)obj["IsBinaryAnswer"]).Value.ToString());
            }

            if (obj["Dependency"] != null && obj["Dependency"].HasValues)
            {
                JObject dependencyObj = JObject.FromObject(obj["Dependency"]);
                if (dependencyObj != null)
                {
                    Dependency.ReadJson(dependencyObj);
                }
            }

            ObjectState = ObjectStates.None;
            SetOriginal();
        }


    }
}
