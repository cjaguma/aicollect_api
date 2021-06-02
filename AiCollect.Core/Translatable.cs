using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Translatable : AiCollectObject
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Translations Translations { get; private set; }

        public Translatable()
        {
            Translations = new Translations();
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

            //Questionaires
            if (obj["Translations"] != null)
            {
                JArray dataListObj = JArray.FromObject(obj["Translations"]);
                if (dataListObj != null)
                    Translations.ReadJson(obj);
            }
        }

    }
}
