using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class Parish : LocationObj
    {
        public string Name { get; set; }

        public List<Village> Villages { get; set; }

        public Parish()
        {
            Villages = new List<Village>();
        }

    }
}
