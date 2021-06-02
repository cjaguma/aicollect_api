using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Regions : AiCollectObject, IEnumerable<Region>
    {
        private List<Region> _regions;

        public Regions()
        {
            _regions = new List<Region>();
        }

        public Region Add()
        {
            Region region = new Region();
            _regions.Add(region);
            return region;
        }

        public void Clear()
        {
            if (_regions.Count > 0)
                _regions.Clear();
        }

        public override void Cancel()
        {

        }

        public IEnumerator<Region> GetEnumerator()
        {
            foreach (var b in _regions)
                yield return b;
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

        public void ReadJson(JArray jArrray)
        {
            if (jArrray == null) return;
            foreach (JObject regionObj in jArrray)
            {
                var region = Add();
                region.ReadJson(regionObj);
            }
        }
    }
}
