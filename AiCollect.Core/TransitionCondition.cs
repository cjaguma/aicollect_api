
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    public enum TransitionTypes
    {
        Navigation,
        ConditionalNavigation
    }

    [DataContract]
    public class TransitionCondition : ValidationCondition
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
        private TransitionTypes _transitionType;

        [DataMember]
        public TransitionTypes TransitionType
        {
            get { return _transitionType; }
            set
            {
                if (value != _transitionType)
                {
                    _transitionType = value;
                    ObjectState = ObjectStates.Modified;
                    
                }
            }
        }

        [DataMember]
        public string TargetDataObjectKey { get; set; }

        private DataCollectionObject _dataForm;

        public DataCollectionObject DataObject
        {
            get
            {
                if (_dataForm == null && !string.IsNullOrWhiteSpace(TargetDataObjectKey))
                {
                    _dataForm = Configuration.Questionaires.ByKey(TargetDataObjectKey);
                }
                return _dataForm;
            }
            set
            {
                if (value != _dataForm)
                {
                    _dataForm = value;
                    if (_dataForm != null)
                        TargetDataObjectKey = _dataForm.Key;
                    else
                        TargetDataObjectKey = null;
                }
            }
        }

      
        [DataMember]
        public string AttributeKey { get; set; }

      

        public new TransitionConditions Parent
        {
            get
            {
                return (TransitionConditions)base.Parent;
            }
        }

        protected new TransitionCondition OriginalValues { get; set; }

        public TransitionCondition(AiCollectObject parent)
            : base(parent)
        {
            TargetDataObjectKey = string.Empty;

            SetOriginalValues();
        }

        private new void SetOriginalValues()
        {
            base.SetOriginalValues();
            OriginalValues = (TransitionCondition)Copy();
            ObjectState = ObjectStates.None;
        }

      

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            TransitionType = (TransitionTypes)Enum.Parse(typeof(TransitionTypes), ((JValue)obj["TransitionType"]).Value.ToString());
            TargetDataObjectKey = ((JValue)obj["TargetDataObjectKey"]).Value.ToString();
            AttributeKey = ((JValue)obj["AttributeKey"]).Value.ToString();
        }

     
        public override void Update()
        {
            Validate();
            if (ObjectState == ObjectStates.Added)
            {
                //Add transitionlink
                //TransitionLink link = Parent.Parent.Attributes.AddTransitionLink(Key, "Transition_" + DataObject.TableName, "Ref_Transition_" + Parent.Parent.TableName, DataObject);
                //link.LinkType = LinkedAttributeTypes.Transition;
                //link.Update();

                ////Set the new order
                //DataObject.Order = Parent.Parent.Order + 1;
                //DataObject.Update();
            }

            base.Update();
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.None:
                    break;

                case ObjectStates.Removed:
                    break;

                case ObjectStates.Modified:
                case ObjectStates.Added:
                    if (string.IsNullOrWhiteSpace(TargetDataObjectKey))
                        throw new Exception("");
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
                base.Cancel();
                TargetDataObjectKey = OriginalValues.TargetDataObjectKey;
                _transitionType = OriginalValues._transitionType;
                ObjectState = ObjectStates.None;
                
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Transition({0})", TransitionType.ToString()));
            if (TransitionType == TransitionTypes.Navigation)
            {
                sb.Append(string.Format("{0}------>{1}", Parent.Parent.Name, DataObject.Name));
            }

            return sb.ToString();
        }

        public override int CompareTo(AiCollectObject other)
        {
            int result = 0;
            TransitionCondition condition = other as TransitionCondition;
            if (base.CompareTo(condition) == 1)
            {
                if (TransitionType != condition.TransitionType)
                    result = 0;
                else result = 1;
            }
            else
                result = 0;
            return result;
        }
    }
}