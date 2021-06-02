using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace AiCollect.Core
{
    [DataContract]
    public class ClosedQuestion : ChoiceQuestion
    {


        public ClosedQuestion(AiCollectObject parent) : base(parent)
        {
            SetOriginal();
        }

        public override void Validate()
        {

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

        }

        public override JObject ToJson()
        {
            var jObject = base.ToJson();
            return jObject;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
        }

        private ClosedQuestion _original;
        internal override void SetOriginal()
        {
            _original = Copy() as ClosedQuestion;
        }

    }

    public class ChoiceQuestion : Question
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
        public EnumList EnumList { get; set; }

        [DataMember]
        public string EnumListKey { get; set; }
        [DataMember]
        public string EnumListValueKey { get; set; }

        public ChoiceQuestion(AiCollectObject parent) : base(parent)
        {
            EnumList = new EnumList(this);
        }

        public override JObject ToJson()
        {
            var jObject = base.ToJson();

            jObject.Add("EnumList", JObject.FromObject(EnumList));
            return jObject;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
            {
                Name = ((JValue)obj["Name"]).Value.ToString();
            }
            try
            {
                if (obj["EnumList"] != null)
                {
                    JObject jObjectEnumList = JObject.FromObject(obj["EnumList"]);
                    EnumList.ReadJson(jObjectEnumList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ObjectState = ObjectStates.None;
            SetOriginal();
        }


        public override int Compare(Question other)
        {
            var chkBase = base.Compare(other) == 1;
            var chkName = this.Name == other.Name;

            return chkBase && chkName ? 1 : 0;
        }
        public override void Update()
        {
            base.Update();
        }

    }
}
