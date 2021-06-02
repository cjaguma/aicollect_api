using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class District:LocationObj
    {
        public string Name { get; set; }

        public List<County> Counties { get; set; }

        public District()
        {
            Counties = new List<County>();
        }


    }

    public abstract class LocationObj
    {
        public string Code { get; set; }
        public int OID { get; set; }
        public string Key { get; set; }
        public LocationObj() 
        {
            Key = Guid.NewGuid().ToString();
        }

    }
}
