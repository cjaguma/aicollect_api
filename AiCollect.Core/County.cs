using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class County : LocationObj
    {
        public string Name { get; set; }

        public List<SubCounty> SubCounties { get; set; }

        public County()
        {
            SubCounties = new List<SubCounty>();
        }
    }
}
