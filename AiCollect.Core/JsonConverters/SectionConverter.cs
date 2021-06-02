using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.JsonConverters
{
    public class SectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Section);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Section))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    var section = new Section(null);
                    section.ReadJson(obj);
                    return section;
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
