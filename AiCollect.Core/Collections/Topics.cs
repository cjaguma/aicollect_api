using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Topics : AiCollectObject, IEnumerable<Topic>
    {

        private List<Topic> _topics;

        public Topics()
        {
            Init();
        }

        public Topics(AiCollectObject parent):base(parent)
        {
            Init();
        }

        private void Init()
        {
            _topics = new List<Topic>();
        }

        public override void Cancel()
        {
            
        }

        public Topic Add()
        {
            Topic topic = new Topic(this);
            _topics.Add(topic);
            return topic;
        }

        public void Remove(Topic topic)
        {
            _topics.Remove(topic);
        }

        public IEnumerator<Topic> GetEnumerator()
        {
            foreach (var tr in _topics)
                yield return tr;
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
            JArray topicsObjs = JArray.FromObject(obj["Topics"]);
            if (topicsObjs != null)
            {
                foreach (var cobj in topicsObjs)
                {
                    var topic = Add();
                    topic.ReadJson((JObject)cobj);
                }
            }
        }

    }
}
