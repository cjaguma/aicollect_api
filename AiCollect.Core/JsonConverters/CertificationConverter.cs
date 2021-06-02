using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.JsonConverters
{
    public class CertificationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Certification);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Certification))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    CertificationTypes certificationType = CertificationTypes.FairTrade;
                    if (obj["CerificationType"] != null && ((JValue)obj["CerificationType"]).Value != null)
                        certificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), ((JValue)obj["CerificationType"]).Value.ToString());
                    var certification = ObjectFactory.CreateCertification(certificationType, null);
                    certification.ReadJson(obj);
                    return certification;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override bool CanWrite { get { return false; } }
    }

    public class LocationObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LocationObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(LocationObject))
            {
                JArray locations = serializer.Deserialize<JToken>(reader) as JArray;
                if (locations != null)
                {
                    foreach (JObject obj in locations)
                    {

                    }
                    //CertificationTypes certificationType = CertificationTypes.FairTrade;
                    //if (obj["CerificationType"] != null && ((JValue)obj["CerificationType"]).Value != null)
                    //    certificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), ((JValue)obj["CerificationType"]).Value.ToString());
                    //var certification = ObjectFactory.CreateCertification(certificationType, null);
                    //certification.ReadJson(obj);
                    //return certification;
                    return null;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override bool CanWrite { get { return false; } }
    }
}
