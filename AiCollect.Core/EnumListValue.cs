using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AiCollect.Core
{
    [DataContract]
    public class EnumListValue : AiCollectObject
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
        [DataMember]
        public string EnumListId { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        private string _description;
        private int _code;

        public EnumListValue() : base()
        {

        }

        public EnumListValue(AiCollectObject parent) : base(parent)
        {

        }

        [Browsable(false)]
        public new EnumListValues Parent
        {
            get
            {
                return (EnumListValues)base.Parent as EnumListValues;
            }
        }

        private EnumListValue _original;

        internal override void SetOriginal()
        {
            _original = Copy() as EnumListValue;

        }



        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        [DataMember]
        public int Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        public override void Cancel()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                    Parent.Remove(this);
                    break;
                case ObjectStates.Modified:
                    Code = _original.Code;
                    Description = _original.Description;
                    break;
            }
        }

        public override int CompareTo(AiCollectObject other)
        {
            EnumListValue inItem = other as EnumListValue;
            if (this.Code.Equals(inItem.Code) && this.Description.Equals(inItem.Description))
                return 1;
            return 0;
        }

        public override void Update()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    Validate();
                    ObjectState = ObjectStates.None;
                    SetOriginal();
                    break;
                case ObjectStates.Removed:
                    break;
            }
        }

        public override void Validate()
        {
            switch (ObjectState)
            {
                case ObjectStates.Added:
                case ObjectStates.Modified:
                    if (string.IsNullOrWhiteSpace(Description))
                        throw new Exception("Description cannot be empty");

                    break;
            }

        }

        public override void ReadJson(JObject obj)
        {
            base.ReadJson(obj);
            if (obj["Code"] != null && ((JValue)obj["Code"]).Value != null)
            {
                Code = Convert.ToInt32(((JValue)obj["Code"]).Value);
            }
            if (obj["Description"] != null && ((JValue)obj["Description"]).Value != null)
            {
                Description = Convert.ToString(((JValue)obj["Description"]).Value);
            }
            ObjectState = ObjectStates.None;
            SetOriginal();
        }

        public int Compare(EnumListValue other)
        {
            var chkCode = this.Code == other.Code;
            var chkDescription = this.Description == other.Description;
            return chkCode && chkDescription ? 1 : 0;
        }
    }
}
