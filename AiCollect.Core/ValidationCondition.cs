
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace AiCollect.Core
{
    public enum ValidationTypes
    {
        Value,
        Length
    }

    [DataContract]
    public class ValidationCondition : AiCollectObject
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
        protected ValidationTypes _validationType;

        [DataMember]
        public ValidationTypes ValidationType
        {
            get { return _validationType; }
            set
            {
                if (value != _validationType)
                {
                    _validationType = value;
                    ObjectState = ObjectStates.Modified;
                    //OnPropertyChanged("ValidationType");
                }
            }
        }

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
                    //OnPropertyChanged("Qualifier");
                }
            }
        }

        private string _validationValue;

        [DataMember]
        public string ValidationValue
        {
            get { return _validationValue; }
            set
            {
                if (value != _validationValue)
                {
                    _validationValue = value;
                    ObjectState = ObjectStates.Modified;
                    //OnPropertyChanged("ValidationValue");
                }
            }
        }

        //public new ValidationConditions Parent
        //{
        //    get
        //    {
        //        return (ValidationConditions)base.Parent;
        //    }
        //}

        protected ValidationCondition OriginalValues { get; private set; }

        public ValidationCondition(AiCollectObject parent)
            : base(parent)
        {
            ValidationValue = string.Empty;
            SetOriginalValues();
        }

        protected void SetOriginalValues()
        {
            OriginalValues = (ValidationCondition)Copy();
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
                    if (String.IsNullOrWhiteSpace(ValidationValue))
                        throw new Exception("Validation value is empty");
                    break;

                default:
                    throw new Exception("Unknown EditMode in Validate()");
            }
        }

        public override void Cancel()
        {
            if (ObjectState == ObjectStates.Added)
            {
                ((ValidationConditions)Parent).InternalRemove(this);
            }
            else if (ObjectState != ObjectStates.None)
            {
                _qualifier = OriginalValues._qualifier;
                _validationType = OriginalValues._validationType;
                _validationValue = OriginalValues._validationValue;
            }
        }

        public override void Update()
        {
            Validate();
            if (ObjectState == ObjectStates.Removed)
            {
                ((ValidationConditions)Parent).InternalRemove(this);
               
            }
            else
            {
                if (ObjectState == ObjectStates.Added)
                {
                    //OnAdded();
                }
                else if (ObjectState == ObjectStates.Modified)
                {
                    //OnChanged();
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
            int result = 0;
            ValidationCondition condition = other as ValidationCondition;
            if (Qualifier != condition.Qualifier && ValidationValue != condition.ValidationValue)
            {
                result = 0;
            }
            else
                result = 1;
            return result;
        }
    }
}