using AiCollect.Data;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Datalabs.Data
{
    public class dloReport
    {
        #region Members
       // private Report _report;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the Key of the report
        /// </summary>
       // public string Key { get { return _report.Key; } }
        /// <summary>
        /// Gets the Name of the report
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// Gets the description of the report
        /// </summary>
       // public string Description { get { return _report.Description; } }

        public dloDataApplication Application { get; internal set; }

       // public Report BaseReport { get { return _report; } }

       // public ReportDataSourceTypes DataSourceType { get { return _report.DataSourceType; } }
        #endregion

        //public dloReport(Report report,dloDataApplication app)
        //{
        //    _report = report;

        //    Application = app;

        //    Name = _report.Name;
        //}

    
        
    }
}
