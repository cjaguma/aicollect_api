using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Modules : AiCollectObject,IEnumerable<Module>
    {
        private List<Module> _modules;
        public Modules()
        {
            _modules = new List<Module>();
        }
        public IEnumerator<Module> GetEnumerator()
        {
            foreach(var m in _modules)
            {
                yield return m;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
        }

        public Module Add()
        {
            Module module = new Module();
            _modules.Add(module);
            return module;
        }
    }
}
