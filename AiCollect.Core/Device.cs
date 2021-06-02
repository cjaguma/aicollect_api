using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    [DataContract]
    public class Device:AiCollectObject
    { 

        private string _imei;
        private string _name;
        private string _id;
        [DataMember]
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private Status _status;

        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                if(_status!=value)
                {
                    _status = value;
                    ObjectState = ObjectStates.Modified;
                }
            }
        }
        public Device(Devices parent):base(parent)
        {
            Id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public string Imei { get => _imei; set => _imei = value; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if(_name!=value)
                {
                    _name = value;
                }
            }
        }

        public override void Cancel()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Validate()
        {
            
        }
    }
}
