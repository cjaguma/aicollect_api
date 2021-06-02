using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class Templates : AICollect, IEnumerable<Template>
    {
        private List<Template> _templates;
        
        public Templates():base(null)
        {
            _templates = new List<Template>();
        }

        public Template Add()
        {
            Template template = new Template();
            _templates.Add(template);
            return template;
        }

        public IEnumerator<Template> GetEnumerator()
        {
            foreach (var template in _templates)
                yield return template;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
