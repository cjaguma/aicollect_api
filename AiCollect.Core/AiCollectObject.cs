using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public abstract class AiCollectObject
    {
        [Browsable(false)]
        public Configuration Configuration { get; set; }
        [Browsable(false)]
        public AiCollectObject Parent { get; private set; }

        [DataMember]
        public string Key
        {
            get; set;
        }
        protected bool _isDirty;
        [Browsable(false)]

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                if (value != _isDirty)
                {
                    _isDirty = value;
                }
            }
        }
        private ObjectStates _objectState;
        [Browsable(false)]
        /// <summary>
        /// Gets/Sets the object state in memory
        /// </summary> 
        public ObjectStates ObjectState
        {
            get
            {
                return _objectState;
            }
            set
            {
                if (_objectState != value)
                {
                    if (value < _objectState || _objectState == ObjectStates.None)
                    {
                        _objectState = value;
                    }
                }
            }
        }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public int OID
        {
            get; set;
        }
        [DataMember]
        
        public int Deleted { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        public AiCollectObject(AiCollectObject parent)
        {
            if (parent != null)
            {
                if (parent is Configuration)
                {
                    Configuration = (Configuration)parent;
                }
                else
                {
                    Configuration = parent.Configuration;
                }
            }
            Key = Guid.NewGuid().ToString();
            Parent = parent;
        }
        public AiCollectObject()
        {
            Key = Guid.NewGuid().ToString();
        }
        public virtual int CompareTo(AiCollectObject other)
        {
            if (Key.Equals(other.Key))
                return 1;
            return 0;
        }

        public virtual void ReadJson(JObject obj)
        {
            if (obj["Key"] != null && ((JValue)obj["Key"]).Value != null)
                Key = ((JValue)obj["Key"]).Value.ToString();

            if (obj["OID"] != null && ((JValue)obj["OID"]).Value != null)
                OID = int.Parse(((JValue)obj["OID"]).Value.ToString());

            if (obj["Deleted"] != null && ((JValue)obj["Deleted"]).Value != null)
            {
                var deleted = ((JValue)obj["Deleted"]).Value.ToString();
                int d = 0;
                int.TryParse(deleted, out d);
                Deleted = d;
            }

            if (obj["ClientName"] != null && ((JValue)obj["ClientName"]).Value != null)
                ClientName = ((JValue)obj["ClientName"]).Value.ToString();

        }

        public virtual JObject ToJson()
        {
            JObject jObject = new JObject();
            jObject.Add("Key", Key);
            return jObject;
        }


        internal AiCollectObject Copy()
        {
            return (AiCollectObject)this.MemberwiseClone();
        }

        public AiCollectObject Clone()
        {
            return (AiCollectObject)this.MemberwiseClone();
        }

        public void SetKey(string key)
        {
            Key = key;
        }
        public abstract void Validate();

        public abstract void Cancel();

        public abstract void Update();

        internal virtual void SetOriginal()
        {

        }


    }

}
