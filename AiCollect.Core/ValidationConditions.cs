using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace AiCollect.Core
{
    public enum Validity
    {
        All,
        Any,
        None,
        NotAll
    }

    [DataContract]
    public class ValidationConditions : AiCollectObject, IEnumerable<ValidationCondition>
    {
        private List<ValidationCondition> _conditions;

        public int Count
        {
            get
            {
                return _conditions.Count;
            }
        }

   
        public ValidationConditions(AiCollectObject parent)
            : base(parent)
        {
            _conditions = new List<ValidationCondition>();
        }

        public ValidationCondition Add()
        {
            ValidationCondition logic = new ValidationCondition(this);
            logic.ObjectState = ObjectStates.Added;
           
            _conditions.Add(logic);

            return logic;
        }

    
        public IEnumerator<ValidationCondition> GetEnumerator()
        {
            foreach (ValidationCondition condition in _conditions)
            {
                if (condition.ObjectState != ObjectStates.Removed)
                    yield return condition;
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

        internal void InternalRemove(ValidationCondition condition)
        {
            _conditions.Remove(condition);
        }
    }
}