using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Translation : AiCollectObject
    {
        [DataMember]
        public Language Language { get; set; }

        [DataMember]
        public string TranslatedText { get; set; }

        public Translation()
        {
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

            if (obj["TranslatedText"] != null && ((JValue)obj["TranslatedText"]).Value != null)
                TranslatedText = ((JValue)obj["TranslatedText"]).Value.ToString();

            //Language
            if (obj["Language"] != null)
            {
                var languageObj = JObject.FromObject(obj["Language"]);
                if (languageObj != null)
                    Language.ReadJson(languageObj);
            }

        }

    }
}
