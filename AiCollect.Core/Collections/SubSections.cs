using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class SubSections : AiCollectObject,IEnumerable<SubSection>,ICollection<SubSection>
    {

        public IReadOnlyCollection<SubSection> List
        {
            get
            {
                return _subSections.AsReadOnly();
            }
        }


        private List<SubSection> _subSections;

        public new Section Parent
        {
            get
            {
                return base.Parent as Section;
            }
        }


        public SubSection this[int index]
        {
            get
            {
                return _subSections[index];
            }
        }

        public int Count
        {
            get
            {
                return _subSections.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public SubSections(AiCollectObject parent) : base(parent)
        {
            _subSections = new List<SubSection>();
        }

        public SubSections() : base()
        {
            _subSections = new List<SubSection>();
        }

        public override void Cancel()
        {
           
        }

        public IEnumerator<SubSection> GetEnumerator()
        {
            foreach (var s in _subSections)
                yield return s;
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

        public void Add(SubSection item)
        {
            _subSections.Add(item);
        }

        public SubSection Add()
        {
            SubSection subSection = new SubSection(this);
            _subSections.Add(subSection);
            return subSection;
        }

        public void Clear()
        {
            _subSections.Clear();
        }

        public bool Contains(SubSection item)
        {
            return _subSections.Contains(item);
        }

        public void CopyTo(SubSection[] array, int arrayIndex)
        {
           
        }

        public bool Remove(SubSection item)
        {
            return _subSections.Remove(item);
        }

    }
}
