using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    //Severity Levels
    //1 - information
    //2 - System error
    //3 - Security error
    //4 - Critical system error
    public class dloApplicationLog
    {
        #region Members
        private dloDataApplication _application;
        #endregion
        #region Properties
        public string Id { get; set; }
        public int Severity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string DeviceName { get; set; }
        public string Code { get; set; }
        public string Msg { get; set; }
        #endregion

        internal dloApplicationLog(dloDataApplication application)
        {
            _application = application;
            Id = Guid.NewGuid().ToString();
            Severity = 1;
            CreatedOn = DateTime.Now;
           // CreatedBy = _application.CurrentUser.FullName;
        }


        internal void Save()
        {
            DbCommand cmd = _application.DbInfo.Connection.CreateCommand();

            string sql = "INSERT INTO dsto_application_log(Guid,Created_On,Created_By,DeviceName,Code,Msg,Severity,Deleted) VALUES (@id,@createdon,@createdby,@device,@code,@msg,@severity,0)";
            cmd.CommandText = sql;

            DbParameter idPar = cmd.CreateParameter();
            idPar.ParameterName = "@id";
            idPar.Value = Id;
            cmd.Parameters.Add(idPar);

            DbParameter createdOnPar = cmd.CreateParameter();
            createdOnPar.ParameterName = "@createdon";
            createdOnPar.Value =CreatedOn;
            cmd.Parameters.Add(createdOnPar);

            DbParameter createdByPar = cmd.CreateParameter();
            createdByPar.ParameterName = "@createdby";

            if (string.IsNullOrWhiteSpace(CreatedBy))
                createdByPar.Value = CreatedBy;
            //else
            //    createdByPar.Value = _application.CurrentUser.Username;
            cmd.Parameters.Add(createdByPar);
            
            DbParameter devicePar = cmd.CreateParameter();
            devicePar.ParameterName = "@device";
            if (DeviceName != null)
                devicePar.Value = DeviceName;
            else
                devicePar.Value = Environment.MachineName;
            cmd.Parameters.Add(devicePar);

            DbParameter codePar = cmd.CreateParameter();
            codePar.ParameterName = "@code";
            codePar.Value = Code;
            cmd.Parameters.Add(codePar);

            DbParameter msgPar = cmd.CreateParameter();
            msgPar.ParameterName = "@msg";
            msgPar.Value = Msg;
            cmd.Parameters.Add(msgPar);

            DbParameter severityPar = cmd.CreateParameter();
            severityPar.ParameterName = "@severity";
            severityPar.Value = Severity;
            cmd.Parameters.Add(severityPar);

            int res = _application.DbInfo.ExecuteCECommand(cmd);
        }
    }

    
}
