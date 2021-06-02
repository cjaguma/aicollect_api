using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public  class AICollectApp
    {
        public Configuration Configuration { get; private set; }

        private static AICollectApp _instance;

        public static AICollectApp Instance
        {
            get
            {
                if(_instance==null)
                {
                    _instance = new AICollectApp();
                }
                return _instance;
            }
        }

        private  AICollectApp()
        {
            Configuration = new Configuration();
            CurrentUser = new User(null);
        }

        public User CurrentUser
        {
            get;private set;
        }
       
       
    }
}
