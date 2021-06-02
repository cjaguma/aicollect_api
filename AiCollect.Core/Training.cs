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
    [JsonConverter(typeof(TrainingConverter))]
    public class Training : AiCollectObject
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
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private Trainers _trainers;
        private Topics _topics;
        private Trainees _trainees;
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public bool Deleted { get; set; }


        [DataMember]
        public DateTime StartDate 
        {
            get
            { 
                return _startDate; 
            }
            set 
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    ObjectState = ObjectStates.Modified;
                }
                
            }
        }
        [DataMember]
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    ObjectState = ObjectStates.Modified;
                }
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

        [DataMember]
        public Trainers Trainers { get => _trainers; set => _trainers = value; }
        [DataMember]
        public Topics Topics { get => _topics; set => _topics = value; }
        [DataMember]
        public Trainees Trainees { get => _trainees; set => _trainees = value; }

        public Training(AiCollectObject parent):base(parent)
        {
            Init();
        }

        private void Init()
        {
            Trainers = new Trainers(this);
            Topics = new Topics(this);
            Trainees = new Trainees(this);
        }

        public override void Cancel()
        {

        }

        public override void Update()
        {

        }

        public override void Validate()
        {

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["CreatedBy"] != null && ((JValue)obj["CreatedBy"]).Value != null)
                CreatedBy = ((JValue)obj["CreatedBy"]).Value.ToString();

            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();

            if (obj["StartDate"] != null && ((JValue)obj["StartDate"]).Value != null)
                StartDate = DateTime.Parse(((JValue)obj["StartDate"]).Value.ToString());

            if (obj["EndDate"] != null && ((JValue)obj["EndDate"]).Value != null)
                EndDate = DateTime.Parse(((JValue)obj["EndDate"]).Value.ToString());

            if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
                ConfigurationId = int.Parse(((JValue)obj["ConfigurationId"]).Value.ToString());

            _topics.ReadJson(obj);
            _trainers.ReadJson(obj);
            _trainees.ReadJson(obj);
            ObjectState = ObjectStates.None;
            SetOriginal();
        }

    }
}
