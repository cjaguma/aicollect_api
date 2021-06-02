using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Certifications : AICollect, IEnumerable<Certification>
    {
        private List<Certification> _certifications;

        public Certifications(AiCollectObject parent) : base(parent)
        {
            _certifications = new List<Certification>();
        }

        public Certification Add(CertificationTypes type)
        {
            Certification certification = null;
            switch (type)
            {
                case CertificationTypes.FairTrade:
                    certification = new FairTrade(this);
                    break;
                case CertificationTypes.Organic:
                    certification = new Organic(this);
                    break;
                case CertificationTypes.UTZ:
                    certification = new UTZ(this);
                    break;
            }

            if (certification != null)
            {
                certification.ObjectState = ObjectStates.Added;
                _certifications.Add(certification);
            }

            return certification;

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            _certifications.Clear();
            JArray collectionObjs = null;
            if (obj["Certifications"] != null)
            {
                collectionObjs = JArray.FromObject(obj["Certifications"]);
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

                    if (cobj["CerificationType"] != null && ((JValue)cobj["CerificationType"]).Value != null)
                    {
                        var CerificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), ((JValue)cobj["CerificationType"]).Value.ToString());
                        var item = Add(CerificationType);
                        item.ReadJson((JObject)cobj);
                    }
                }
            }
        }

        public void Remove(Certification certification)
        {
            _certifications.Remove(certification);
        }

        public IEnumerator<Certification> GetEnumerator()
        {
            foreach (var c in _certifications)
                yield return c;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
