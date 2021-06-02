using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class SubCounty : LocationObj
    {
        public string Name { get; set; }

        public List<Parish> Parishes { get; set; }

        public SubCounty()
        {
            Parishes = new List<Parish>();
        }
    }
}
