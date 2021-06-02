using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCollect.Core;
using System.Data.Common;
using System.Reflection;

namespace AiCollect.Data
{
    public class dloQuestion : Question
    {
        private dloDataApplication _application;

        internal dloQuestion(dloDataApplication application) : base(null)
        {
            _application = application;

        }

        public dloQuestion(AiCollectObject parent) : base(null)
        {
        }

        

    }
}
