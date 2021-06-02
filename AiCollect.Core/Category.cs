using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Category : AiCollectObject
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
        public string Name { get; set; }
        [DataMember]
        public bool Deleted { get; set; }
        [DataMember]
        public Questionaires Questionaires { get; set; }
        public Category()
        {
            Questionaires = new Questionaires();
        }

        public string QuestionaireId { get; set; }

        public Category(AiCollectObject parent) : base(parent)
        {
            Questionaires = new Questionaires();
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

            Questionaires.Clear();
            Questionaires.ReadJson(obj);
        }
    }
}
