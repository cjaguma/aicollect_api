using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalabs.Data
{
    /// <summary>
    /// Class to hold collection of reports
    /// </summary>
    public class dloReportCollection : ObservableCollection<dloReport>
    {
        public dloReport this[string name]
        {
            get { return this.FirstOrDefault(r => r.Name == name); }
        }
    }
}
