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
    [JsonConverter(typeof(FieldInspectionConverter))]
    public class FieldInspection : AiCollectObject
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

        private string _fieldName;
        private Questionaire _farmer;
        private Sections _sections;
        private bool _deleted;
        [DataMember]
        public new bool Deleted
        {
            get
            {
                return base.Deleted == 1;
            }
            set
            {
                _deleted = value;
                base.Deleted = _deleted ? 1 : 0;
            }
        }

        [DataMember]
        public string Template { get; set; }
        public Questionaire Farmer { get => _farmer; set => _farmer = value; }
        private string _farmerKey;
        [DataMember]
        public string FarmerKey
        {
            get
            {
                return _farmerKey;
            }
            set
            {
                _farmerKey = value;
            }
        }
        [DataMember]
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                if (_fieldName != value)
                {
                    _fieldName = value;
                }
            }
        }
        [DataMember]
        public Sections Sections { get => _sections; set => _sections = value; }

        [DataMember]
        public List<string> Coordinates { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public Statuses Status { get; set; }

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


        public new FieldInspections Parent
        {
            get
            {
                return base.Parent as FieldInspections;
            }
        }
        public FieldInspection() : base(null)
        {
            DateTime = DateTime.Now;
            Status = Statuses.New;
            Sections = new Sections(this);
            Coordinates = new List<string>();
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

            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

            if (obj["DateTime"] != null && ((JValue)obj["DateTime"]).Value != null)
                DateTime = DateTime.Parse(((JValue)obj["DateTime"]).Value.ToString());

            if (obj["FarmerKey"] != null && ((JValue)obj["FarmerKey"]).Value != null)
                FarmerKey = ((JValue)obj["FarmerKey"]).Value.ToString();


            if (obj["FieldName"] != null && ((JValue)obj["FieldName"]).Value != null)
                FieldName = ((JValue)obj["FieldName"]).Value.ToString();


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

    }
}
