using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.JsonConverters
{
    public class FieldInspectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldInspection);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(FieldInspection))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    var inspection = new FieldInspection();
                    inspection.ReadJson(obj);
                    return inspection;
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

    public class FieldInspectionsConverter : JsonConverter
    {
        public FieldInspectionsConverter()
        {

        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldInspections);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(FieldInspections))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    JArray objArray = JArray.FromObject(obj["Data"]);
                    if (objArray != null)
                    {
                        var inspections = new FieldInspections();
                        inspections.ReadJson(obj);
                        return inspections;
                    }
                }

            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override bool CanWrite { get { return false; } }
    }
}
