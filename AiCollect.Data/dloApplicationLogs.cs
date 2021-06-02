using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloApplicationLogs : ObservableCollection<dloApplicationLog>
    {
        #region Members
        private dloDataApplication _application;
        #endregion

        #region Constructors
        internal dloApplicationLogs(dloDataApplication application)
        {
            _application = application;
        }
        #endregion

        internal dloApplicationLog Add()
        {
            dloApplicationLog log = new dloApplicationLog(_application);
            base.Add(log);
            return log;
        }

        internal void Load()
        {
            string sql ="SELECT * FROM dsto_application_log";

             DataTable table = new System.Data.DataTable();
            _application.DbInfo.ExecuteQuery(sql, table);

            foreach (DataRow dr in table.Rows)
            {
                dloApplicationLog log = Add();
                
                log.Id = Convert.ToString(dr["Guid"]);
                log.CreatedBy = (string)dr["Created_By"];
                log.CreatedOn = (DateTime)dr["Created_On"];
                log.DeviceName = (string)dr["DeviceName"];
                log.Code = (string)dr["Code"];
                log.Msg = (string)dr["Msg"];

                int severity = 0;
                int.TryParse(dr["Severity"].ToString(),out severity);
                log.Severity = severity;
            }
        }
    }
}
