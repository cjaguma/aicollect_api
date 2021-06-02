using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Purchases : AiCollectObject, IEnumerable<Purchase>
    {

        private List<Purchase> _purchases;

        public Purchases()
        {
            Init();
        }

        public Purchases(AiCollectObject parent):base(parent)
        {
            Init();
        }

        public Purchase Add()
        {
            Purchase purchase = new Purchase(this);
            purchase.ObjectState = ObjectStates.Added;
            _purchases.Add(purchase);

            return purchase;
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);

            _purchases.Clear();
            JArray collectionObjs = null;
            if (obj["Purchases"] != null)
            {
                collectionObjs = JArray.FromObject(obj["Purchases"]);
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

        public void Remove(Purchase purchase)
        {
            _purchases.Remove(purchase);
        }

        private void Init()
        {
            _purchases = new List<Purchase>();
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Purchase> GetEnumerator()
        {
            foreach (var f in _purchases)
                yield return f;
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

    }
}
