using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.JsonConverters
{
    public class TraineeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Trainee);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Trainee))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    var trainee= new Trainee(null);
                    trainee.ReadJson(obj);
                    return trainee;
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
