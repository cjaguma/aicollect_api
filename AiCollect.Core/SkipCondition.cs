
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace AiCollect.Core
{
    public enum Qualifiers
    {
        IsEqualTo,
        IsNotEqualTo,
        IsGreaterThan,
        IsLessThan,
        IsNull,
        IsNotNull,
        StartsWith,
        DoesNotStartWith,
        Contains,
        DoesNotContain
    }

    public enum FieldStates
    {
        None,
        Enable,
        Disable,
        Show,
        Hide
    }

    [DataContract]
    public class SkipCondition : AiCollectObject
    {
        [DataMember]
        public new string Key
        {
            get
            {
                return base.Key;
            }
            set
            {
                base.Key = value;
            }
        }
        [DataMember]
        public new int OID
        {
            get
            {
                return base.OID;
            }
            set
            {
                base.OID = value;
            }
        }

        private Question _question;
        private EnumListValue _answer;
        private EnumList _enumList;

        private Target _target;

        [DataMember]
        public string AttributeKey { get; set; }

        [DataMember]
        public string QuestionKey { get; set; }

        [DataMember]
        public new bool Deleted { get; set; }

        protected Qualifiers _qualifier;

        [DataMember]
        public Qualifiers Qualifier
        {
            get { return _qualifier; }
            set
            {
                if (value != _qualifier)
                {
                    _qualifier = value;
                    ObjectState = ObjectStates.Modified;

                }
            }
        }

        [DataMember]
        public DataCollectionObectTypes DataCollectionObectType { get; set; }

        [DataMember]
        public EnumListValue Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public Target Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        public new SkipConditions Parent
        {
            get
            {
                return (SkipConditions)base.Parent;
            }
        }

        protected SkipCondition OriginalValues { get; private set; }
        [DataMember]
        public EnumList EnumList
        {
            get
            {
                return _enumList;
            }
            set
            {
                if (_enumList != value)
                    _enumList = value;
            }
        }

        public SkipCondition(AiCollectObject parent)
            : base(parent)
        {
            Answer = new EnumListValue(null);
            Init();
        }

        private void Init()
        {
            Target = new Target();
        }

        private void SetOriginalValues()
        {
            OriginalValues = (SkipCondition)Copy();
            ObjectState = ObjectStates.None;
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.None:
                    break;

                case ObjectStates.Removed:
                    break;

                case ObjectStates.Added:
                case ObjectStates.Modified:
                    //if (String.IsNullOrWhiteSpace(AttributeKey))
                    //    throw new Exception("key is empty");
                    //if (String.IsNullOrWhiteSpace(DataFieldValue))
                    //    throw new DataFieldValueCannotBeEmptyException(this);
                    break;

                default:
                    throw new Exception("Unknown EditMode in Validate()");
            }
        }

        public override void Cancel()
        {
            if (ObjectState == ObjectStates.Added)
            {
                Remove();
            }
            else if (ObjectState != ObjectStates.None)
            {
                _qualifier = OriginalValues._qualifier;
                //_dataFieldValue = OriginalValues._dataFieldValue;
            }
        }

        public override void Update()
        {
            Validate();
            if (ObjectState == ObjectStates.Removed)
            {
                Parent.InternalRemove(this);

            }
            else
            {
                if (ObjectState == ObjectStates.Added)
                {

                }
                else if (ObjectState == ObjectStates.Modified)
                {

                }

                SetOriginalValues();
            }
        }

        public void Remove()
        {
            ObjectState = ObjectStates.Removed;
        }

        public override int CompareTo(AiCollectObject other)
        {
            SkipCondition condition = this;
            SkipCondition condition1 = other as SkipCondition;
            var sameName = condition.AttributeKey.Equals(condition1.AttributeKey);
            var sameQualifier = condition.Qualifier.Equals(condition1.Qualifier);
            //var sameAttributeValue = condition.AttributeValue.Equals(condition1.AttributeValue);
            return sameName ? 1 : 0;
        }

    }
}