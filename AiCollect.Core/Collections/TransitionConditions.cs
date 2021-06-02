using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace AiCollect.Core
{
    [DataContract]
    public class TransitionConditions : AiCollectObject, IEnumerable<TransitionCondition>,ICollection<TransitionCondition>
    {

        private List<TransitionCondition> _conditions;

        public int Count
        {
            get
            {
                return _conditions.Count;
            }
        }

        public new Section Parent
        {
            get
            {
                return (Section)base.Parent;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TransitionConditions(AiCollectObject parent)
            : base(parent)
        {
            _conditions = new List<TransitionCondition>();
        }

        public TransitionCondition Add()
        {
            TransitionCondition condition = new TransitionCondition(this);
            condition.ObjectState = ObjectStates.Added;        
            _conditions.Add(condition);
            return condition;
        }

       
        public IEnumerator<TransitionCondition> GetEnumerator()
        {
            foreach (TransitionCondition condition in _conditions)
            {
                if (condition.ObjectState != ObjectStates.Removed)
                    yield return condition;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            _conditions.Clear();
            JArray conditionsObj = JArray.FromObject(obj["TransitionConditions"]);
            if (conditionsObj!=null)
            {
                foreach(JObject conditionObj in conditionsObj)
                {
                    var condition = Add();
                    condition.ReadJson(conditionObj);
                }
            }
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

        internal void InternalRemove(TransitionCondition condition)
        {
            _conditions.Remove(condition);
        }

        public TransitionCondition ByKey(string key)
        {
            return _conditions.Find(f => f.Key == key);
        }

        public override int CompareTo(AiCollectObject other)
        {
            int result = 0;
            TransitionConditions conditions = other as TransitionConditions;
            if (_conditions.Count > 0 && conditions.Count > 0)
            {
                foreach (var condition in _conditions)
                {
                    var importedCondition = conditions.ByKey(condition.Key);
                    if (importedCondition != null)
                    {
                        if (importedCondition.CompareTo(condition) == 1)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                            break;
                        }
                    }
                }
            }
            else
                result = 1;
            return result;
        }

        public void Add(TransitionCondition item)
        {
            _conditions.Add(item);
        }

        public void Clear()
        {
            _conditions.Clear();
        }

        public bool Contains(TransitionCondition item)
        {
            return _conditions.Contains(item);
        }

        public void CopyTo(TransitionCondition[] array, int arrayIndex)
        {
            
        }

        public bool Remove(TransitionCondition item)
        {
            return _conditions.Remove(item);
        }

    }
}