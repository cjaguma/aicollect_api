using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(TemplateConverter))]
    public class Template : AiCollectObject
    {
        private Sections _sections;
        private string _name;
        private string _description;
        private TemplateTypes _templateType;
        private Category _category;
        [DataMember]
        public Category Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }
        [DataMember]
        public TemplateTypes TemplateType
        {
            get
            {
                return _templateType;
            }
            set
            {
                if (_templateType != value)
                {
                    _templateType = value;
                }
            }
        }

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
        public Sections Sections { get => _sections; set => _sections = value; }

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
                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public Template()
        {
            Init();
        }

        public Template(AiCollectObject parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            Sections = new Sections();
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

            if (obj["Description"] != null && ((JValue)obj["Description"]).Value != null)
                Description = ((JValue)obj["Description"]).Value.ToString();

            if (obj["TemplateType"] != null && ((JValue)obj["TemplateType"]).Value != null)
                TemplateType = (TemplateTypes)Enum.Parse(typeof(TemplateTypes),((JValue)obj["TemplateType"]).Value.ToString());

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
                Deleted = bool.Parse(((JValue)obj["Deleted"]).Value.ToString());

            if (obj["Category"] != null)
            {
                var categoryObj = JObject.FromObject(obj["Category"]);
                if (categoryObj != null)
                {
                    Category = new Category(null);
                    Category.OID = int.Parse(((JValue)categoryObj["OID"]).Value.ToString());
                    Category.Key = ((JValue)categoryObj["Key"]).Value.ToString();
                    Category.Name = ((JValue)categoryObj["Name"]).Value.ToString();
                }
            }

            _sections.Clear();
            _sections.ReadJson(obj);
            ObjectState = ObjectStates.None;
            SetOriginal();

        }

        public override JObject ToJson()
        {
            JObject templateObj = JObject.FromObject(this);
            return templateObj;
        }


    }
}
