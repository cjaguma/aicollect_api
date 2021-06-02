using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AiCollect.Core
{
    [DataContract]
    public class Coordinate:AiCollectObject
    {
        private string _x;
        private string _y;

        public string X { get => _x; set => _x = value; }
        public string Y { get => _y; set => _y = value; }

        public Coordinates Coordinates
        {
            get
            {
                return base.Parent as Coordinates;
            }
        }

        public Coordinate(Coordinates parent):base(parent)
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
