using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class DataLinks:AiCollectObject, IEnumerable<DataLink>
    {
        private List<DataLink> _links;
       
        public DataLinks()
        {
            _links = new List<DataLink>();
        }
        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
        public DataLink Add()
        {
            DataLink dataLink = new DataLink();
            _links.Add(dataLink);
            return dataLink;
        }

        public IEnumerator<DataLink> GetEnumerator()
        {
            foreach (var dLink in _links)
                yield return dLink;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
           
            _links.Clear();
            JArray collectionObjs = null;
            if (obj["DataLinks"] != null)
            {
                collectionObjs = JArray.FromObject(obj["DataLinks"]);
            }
           

            if (collectionObjs != null)
            {
                foreach (var cobj in collectionObjs)
                {               
                    var item = Add();
                    item.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
