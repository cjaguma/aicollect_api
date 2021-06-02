
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core.Collections
{
    public class Configurations : AiCollectObject, IEnumerable<Configuration>
    {
        private List<Configuration> _configurations;

        public Configurations()
        {
            _configurations = new List<Configuration>();
        }

        public Configuration Add()
        {
            Configuration configuration = new Configuration();
            _configurations.Add(configuration);
            return configuration;
        }

        public override void Cancel()
        {
            
        }

        public IEnumerator<Configuration> GetEnumerator()
        {
            foreach (var b in _configurations
                )
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
    }
}
