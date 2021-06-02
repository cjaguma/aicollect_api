using System;
using System.Collections.Generic;
using System.Text;

namespace AiCollect.Core
{
    public class Synchronization
    {
        public string EndPointUrl { get; set; }
        public Configuration Configuration { get; set; }

        public Configuration ServerConfiguration { get; set; }
        public Synchronization(Configuration configurationIn, Configuration serverConfiguration)
        {
            Configuration = configurationIn;
            ServerConfiguration = serverConfiguration;
        }

        public void GetData()
        {
            
        }

        public int Compare()
        {
            
            return 0;
        }


    }
}
