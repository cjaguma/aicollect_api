using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class SkipConditions : AiCollectObject, IEnumerable<SkipCondition>,ICollection<SkipCondition>
    {

        private List<SkipCondition> _conditions;

        public new DataCollectionObject Parent
        {
            get
            {
                return (DataCollectionObject)base.Parent;
            }
        }

        public int Count
        {
            get
            {
                return _conditions.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public SkipConditions(AiCollectObject parent)
            : base(parent)
        {
            _conditions = new List<SkipCondition>();
        }

        public SkipCondition Add()
        {
            SkipCondition logic = new SkipCondition(this);
            logic.ObjectState = ObjectStates.Added;
           
            _conditions.Add(logic);

            return logic;
        }

      
        public SkipCondition Add(SkipCondition condition)
        {
            //condition.Parent = this;
            _conditions.Add(condition);
            return condition;
        }

        public IEnumerator<SkipCondition> GetEnumerator()
        {
            foreach (SkipCondition logic in _conditions)
            {
                if (logic.ObjectState != ObjectStates.Removed)
                    yield return logic;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        

        public override void Validate()
        {
            for (int i = _conditions.Count - 1; i >= 0; i--)
            {
                _conditions[i].Validate();
            }
        }

        public override void Cancel()
        {
            for (int i = _conditions.Count - 1; i >= 0; i--)
            {
                _conditions[i].Cancel();
            }
        }

        public override void Update()
        {
            for (int i = _conditions.Count - 1; i >= 0; i--)
            {
                _conditions[i].Update();
            }
        }

        internal void InternalRemove(SkipCondition condition)
        {
            _conditions.Remove(condition);
        }

        void ICollection<SkipCondition>.Add(SkipCondition item)
        {
            _conditions.Add(item);
        }

        public void Clear()
        {
            _conditions.Clear();
        }

        public bool Contains(SkipCondition item)
        {
            return _conditions.Contains(item);
        }

        public void CopyTo(SkipCondition[] array, int arrayIndex)
        {
            
        }

        public bool Remove(SkipCondition item)
        {
            return _conditions.Remove(item);
        }

    }
}