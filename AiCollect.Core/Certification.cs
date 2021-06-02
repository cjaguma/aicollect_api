using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(CertificationConverter))]
    public abstract class Certification : AiCollectObject
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
        public string CreatedBy { get; set; }

        private string _name;
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string Template { get; set; }

        private Sections _sections;
        [DataMember]
        public Sections Sections
        {
            get
            {
                return _sections;
            }
            set
            {
                _sections = value;
            }
        }

        [DataMember]
        public CertificationTypes CerificationType { get; set; }

        [DataMember]
        public Statuses Status { get; set; }

        [DataMember]
        public Questions Questions { get; private set; }
        private Questionaire _farmer;
        public Questionaire Farmer { get => _farmer; set => _farmer = value; }
        [DataMember]
        private string _key;
        public string FarmerKey
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        private int _configurationId;

        [DataMember]
        public int ConfigurationId
        {
            get
            {
                return _configurationId;
            }
            set
            {
                _configurationId = value;
            }
        }

        public Certification()
        {
            Init();
        }

        public Certification(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            Questions = new Questions(this);
            _sections = new Sections(this);
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();

            if (obj["FarmerKey"] != null && ((JValue)obj["FarmerKey"]).Value != null)
                FarmerKey = ((JValue)obj["FarmerKey"]).Value.ToString();

            if (obj["Status"] != null && ((JValue)obj["Status"]).Value != null)
                Status = (Statuses)Enum.Parse(typeof(Statuses), ((JValue)obj["Status"]).Value.ToString());

            if (obj["Template"] != null && ((JValue)obj["Template"]).Value != null)
                Template = ((JValue)obj["Template"]).Value.ToString();

            if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
                ConfigurationId = int.Parse(((JValue)obj["ConfigurationId"]).Value.ToString());

            _sections.Clear();
            _sections.ReadJson(obj);
            ObjectState = ObjectStates.None;
            SetOriginal();
        }

        public int Compare(Certification other)
        {
            var chkName = this.Name == other.Name;
            
            return 1;
        }
    }
}
