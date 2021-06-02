using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class Sections : AiCollectObject, IEnumerable<Section>,ICollection<Section>
    {

        private List<Section> _sections;
        public IReadOnlyCollection<Section> List
        {
            get
            {
                return _sections.AsReadOnly();
            }
        }

        public new Questionaire Parent
        {
            get
            {
                return base.Parent as Questionaire;
            }
        }

        public Sections(AiCollectObject parent) : base(parent)
        {
            _sections = new List<Section>();
        }

        public Sections() : base(null)
        {
            _sections = new List<Section>();
        }

        public Section this[int index]
        {
            get
            {
                return _sections[index];
            }
        }

        public int Count
        {
            get
            {
                return _sections.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Section Add()
        {
            Section section = new Section(this);
            _sections.Add(section);
            return section;
        }

        public void Add(Section item)
        {
            _sections.Add(item);
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _sections.Clear();
        }

        public bool Contains(Section item)
        {
            return _sections.Contains(item);
        }

        public void CopyTo(Section[] array, int arrayIndex)
        {
            
        }

        public IEnumerator<Section> GetEnumerator()
        {
            foreach (var s in _sections)
                yield return s;
        }

        public bool Remove(Section item)
        {
            return _sections.Remove(item);
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
            try
            {
                if (obj["Sections"] != null)
                {
                    JArray sectionsObjs = JArray.FromObject(obj["Sections"]);
                    if (sectionsObjs != null)
                    {
                        foreach (var cobj in sectionsObjs)
                        {
                            var section = Add();
                            section.ReadJson((JObject)cobj);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        public Section ByKey(string key)
        {
            return _sections.FirstOrDefault(c => c.Key.Equals(key));
        }

    }
}
