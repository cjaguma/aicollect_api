using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Product : AiCollectObject
    {
        private string _name;

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
                    ObjectState = ObjectStates.Modified;
                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        public Product(AiCollectObject parent):base(parent)
        {

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
