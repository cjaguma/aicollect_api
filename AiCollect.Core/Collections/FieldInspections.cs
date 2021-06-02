using AiCollect.Core.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    [JsonConverter(typeof(FieldInspectionsConverter))]
    public class FieldInspections : AiCollectObject, IEnumerable<FieldInspection>
    {

        private List<FieldInspection> _inspections;

        public FieldInspections()
        {
            _inspections = new List<FieldInspection>();
        }

        public override void Cancel()
        {

        }

        public FieldInspection Add()
        {
            FieldInspection inspection = new FieldInspection();
            inspection.ObjectState = ObjectStates.Added;
            _inspections.Add(inspection);
            return inspection;
        }
        public void Add(FieldInspection fieldInspection)
        {
            fieldInspection.ObjectState = ObjectStates.Added;
            _inspections.Add(fieldInspection);
        }
        public IEnumerator<FieldInspection> GetEnumerator()
        {
            foreach (var i in _inspections)
                yield return i;
        }

        public override void Update()
        {

        }

        public override void Validate()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            _inspections.Clear();
            JArray collectionObjs = null;
            if (obj["Inspections"] != null)
            {
                collectionObjs = JArray.FromObject(obj["Inspections"]);
            }
            else if (obj["CollectionObjects"] != null)
                collectionObjs = JArray.FromObject(obj["CollectionObjects"]);

            if (collectionObjs != null)
            {
                foreach (var cobj in collectionObjs)
                {
                    DataCollectionObectTypes dataCollectionObectType = DataCollectionObectTypes.None;
                    if (cobj["CollectionObjectType"] != null && ((JValue)cobj["CollectionObjectType"]).Value != null)
                    {
                        dataCollectionObectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), ((JValue)cobj["CollectionObjectType"]).Value.ToString());
                    }
                    var item = Add();
                    item.ReadJson((JObject)cobj);
                }
            }
        }
    }
}
