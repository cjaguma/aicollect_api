using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(ModuleConverter))]
    public class Module : AiCollectObject
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name!=value)
                {
                    _name = value;
                }
            }
        }

        //[DataMember]
        //public DataFields DataFields { get; set; }
        [DataMember]
        public Questionaires Questionaires { get; set; }

        [DataMember]
        public DataLinks DataLinks { get; set; }
        [DataMember]
        public int ConfigurationId { get; set; }
        public Module()
        {
            Init();
        }

        public Module(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            //DataFields = new DataFields();
            Questionaires = new Questionaires(this);
            DataLinks = new DataLinks();
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
            if (obj["Name"] != null && ((JValue)obj["Name"]).Value != null)
                Name = ((JValue)obj["Name"]).Value.ToString();
            if (obj["ConfigurationId"] != null && ((JValue)obj["ConfigurationId"]).Value != null)
            {
                int id;
                var parsed = int.TryParse(((JValue)obj["ConfigurationId"]).Value.ToString(),out id);
                ConfigurationId = id;
            }
            Questionaires.ReadJson(obj);
            DataLinks.ReadJson(obj);
        }

    }
}
