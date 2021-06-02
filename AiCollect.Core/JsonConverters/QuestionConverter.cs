using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.JsonConverters
{
    public class QuestionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Question);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(Question))
            {
                JObject obj = serializer.Deserialize<JToken>(reader) as JObject;
                if (obj != null)
                {
                    QuestionTypes qnType = QuestionTypes.None;
                    if (obj["QuestionType"] != null && ((JValue)obj["QuestionType"]).Value != null)
                        qnType = (QuestionTypes)Enum.Parse(typeof(QuestionTypes), ((JValue)obj["QuestionType"]).Value.ToString());
                    var question = ObjectFactory.CreateQuestion(null, qnType);
                    question.ReadJson(obj);
                    return question;
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
