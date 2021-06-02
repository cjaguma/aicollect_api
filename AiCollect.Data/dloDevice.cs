using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Management;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using AiCollect.Core;
//using Datalabs.Data.Services;

namespace AiCollect.Data
{
    [DataContract]
    public class dloDevice : Device
    {
        #region Members
        private dloDataApplication _application;
        #endregion
        #region Properties
        public new dloDevices Parent { get; private set; }
       // public bool IsActivated { get { return Status == 1 ? true : false; } }
        public bool IsLoaded { get; internal set; }

        private string _manufacturer;
        [DataMember]
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; }
        }

        private string _description;
        [DataMember]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                   
                }
            }
        }

        private DateTime? _lastSeen;

        [DataMember]
        public DateTime? LastSeen
        {
            get { return _lastSeen; }
            set
            {
                if (value != _lastSeen)
                {
                    _lastSeen = value;
                   
                }
            }
        }
        #endregion

        #region Constructors
        internal dloDevice(dloDataApplication application):base(null)
        {
            _application = application;

        }

        internal dloDevice(dloDevices parent):base(null)
        {
            Parent = parent;
        }

        public dloDevice():base(null)
        {

        }

        private void CreateParameters(DbCommand cmd)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = "@Id";
           // param.Value = Id;
            cmd.Parameters.Add(param);
            var param1 = cmd.CreateParameter();
            param.ParameterName = "@Name";
            param.Value = Name;
            cmd.Parameters.Add(param1);
            var param2 = cmd.CreateParameter();
            param.ParameterName = "@IMEI";
            param.Value = Imei;
            cmd.Parameters.Add(param2);
            var param3 = cmd.CreateParameter();
            param.ParameterName = "@Description";
            param.Value = Description;
            cmd.Parameters.Add(param3);
            var param4 = cmd.CreateParameter();
            param.ParameterName = "@Status";
          //  param.Value = Status.ToString();
            cmd.Parameters.Add(param4);
            var param5 = cmd.CreateParameter();
            param.ParameterName = "@OS";
          //  param.Value = OS;
            cmd.Parameters.Add(param5);
            var param6 = cmd.CreateParameter();
            param.ParameterName = "@OSVersion";
            //param.Value = OSVersion;
            cmd.Parameters.Add(param6);
            var param7 = cmd.CreateParameter();
            param.ParameterName = "@Manufacturer";
            param.Value = Manufacturer;
            cmd.Parameters.Add(param7);
        }
     
        #endregion

        /// <summary>
        /// Loads details for this specified device
        /// </summary>
        internal void Load(bool refresh=false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Imei))
                    return;

                string sql = string.Format("SELECT * FROM dsto_devices WHERE IMEI ='{0}'",Imei);
                DataTable table = new System.Data.DataTable();
                _application.DbInfo.ExecuteQuery(sql, table);


                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    Description = Convert.ToString(dr["Description"]);
                    //Id = Convert.ToString(dr["Guid"]);
                    //Status = Convert.ToInt32(dr["Status"]);
                    Name = Convert.ToString(dr["Name"]);
                    Imei = Convert.ToString(dr["IMEI"]);
                }
                
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                

            }
            finally
            {
                
            }
        }

      
        public void Refresh()
        {
            Load(true);
        }

        public string SaveToDb()
        {
            var msg = string.Empty;
            DbCommand cmd = null;
            cmd = _application.DbInfo.Connection.CreateCommand();
            switch (_application.Provider)
            {
                case DataProviders.SQL:
                case DataProviders.SQLite:
                    cmd.CommandText = string.Format("SELECT COUNT(*) FROM dsto_devices WHERE IMEI ='{0}' ", Imei);
                    break;
            }
            try
            {
                if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                    _application.DbInfo.Connection.Open();
                int count = (int)_application.DbInfo.ExecuteScalar(cmd);
                if (count == 0)
                {
                    cmd = _application.DbInfo.Connection.CreateCommand();
                    switch (_application.Provider)
                    {
                        case DataProviders.SQL:
                        case DataProviders.SQLite:
                            Status = Status.On;
                            cmd.CommandText = string.Format("INSERT INTO dsto_devices([Guid],[Name],[IMEI],[Description],[Status]) VALUES('{0}','{1}','{2}','{3}','{4}')", Id, Name, Imei, Description, Status);
                            break;
                        case DataProviders.MYSQL:
                            Status = Status.On;
                            cmd.CommandText = string.Format("INSERT INTO dsto_devices([Guid],[Name],[IMEI],[Description],[Status]) VALUES('{0}','{1}','{2}','{3}','{4}')", Id, Name, Imei, Description, Status);
                            break;
                    }

                    //CreateParameters(cmd);
                    if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                        _application.DbInfo.Connection.Open();
                    var result = _application.DbInfo.ExecuteQuery(cmd.CommandText);
                    
                    if (result > -1)
                    {
                       // msg = Strings.DEVICE_SAVED_SUCCESSFULLY;
                    }
                    else
                    {
                       // msg = Strings.DEVICE_NOT_SAVED;
                    }
                }
                else
                {
                   // msg = Strings.DEVICE_ALREADY_EXISTS;
                    //update its status to 1 if not 
                    if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                        _application.DbInfo.Connection.Open();
                    cmd = _application.DbInfo.Connection.CreateCommand();
                    switch (_application.Provider)
                    {
                        case DataProviders.SQL:
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine(string.Format("IF EXISTS(SELECT * FROM dsto_devices WHERE IMEI ='{0}' AND [Status] = 0)",Imei));
                            builder.AppendLine("BEGIN");
                            builder.AppendLine(string.Format("UPDATE dsto_devices SET [Status] = 1 WHERE IMEI ='{0}' ",Imei));
                            builder.AppendLine("END");
                            cmd.CommandText = builder.ToString();
                            break;
                        case DataProviders.SQLite:
                            cmd.CommandText = string.Format("UPDATE dsto_devices SET Status = 1 WHERE IMEI ='{0}' ", Imei);
                            break;
                        case DataProviders.MYSQL:
                            cmd.CommandText = string.Format("UPDATE dsto_devices SET Status = 1 WHERE IMEI ='{0}' ", Imei);
                            break;
                    }
                    if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                        _application.DbInfo.Connection.Open();
                    var res = _application.DbInfo.ExecuteNonQuery(cmd.CommandText);
                    _application.DbInfo.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
               // msg = Strings.DEVICE_NOT_SAVED + " " + ex.Message;
            }
            finally
            {
                _application.DbInfo.Connection.Close();
            }
            return msg;
        }

        public bool DeleteDevice(dloDataApplication application)
        {
            //_application = application;
            //DbCommand cmd = null;
            //bool res = false;
            //try
            //{
            //    cmd = _application.DbInfo.Connection.CreateCommand();
            //    var provider =  _application.Configuration.DataStores.Default.Settings.Provider;


            //    switch (provider)
            //    {
            //        case DataProviders.SQL:
            //        case DataProviders.SQLite:
            //            cmd.CommandText = string.Format("DELETE FROM dsto_devices WHERE IMEI ='{0}'", Imei);
            //            break;
            //    }
            //    if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
            //        _application.DbInfo.Connection.Open();
               
            //    var result = _application.DbInfo.ExecuteQuery(cmd.CommandText);
            //    if (result > -1)
            //    {
            //        res = true;
            //    }
            //    else
            //        res =  false;
            //}
            //catch(Exception ex)
            //{
            //    var h = ex.Message;
            //    res = false;
                
            //}
            //finally
            //{
            //    _application.DbInfo.Connection.Close();
            //}
            return false;
        }

        public bool ActivateDevice(dloDataApplication application)
        {
            //_application = application;
            //DbCommand cmd = null;
            //bool result = false;
            //try
            //{
            //    cmd = _application.DbInfo.Connection.CreateCommand();
            //    var provider = _application.Configuration.DataStores.Default.Settings.Provider;

            //    switch (provider)
            //    {
            //        case DataProviders.SQL:
            //        case DataProviders.SQLite:
            //            cmd.CommandText = string.Format("UPDATE dsto_devices SET Status = 1 WHERE IMEI ='{0}' ", Imei);
            //            break;
            //        case DataProviders.MYSQL:
            //             cmd.CommandText = string.Format("UPDATE dsto_devices SET Status = 1 WHERE IMEI ='{0}' ", Imei);
            //            break;
            //    }
            //    if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
            //        _application.DbInfo.Connection.Open();
            //    var res = _application.DbInfo.ExecuteNonQuery(cmd.CommandText);
            //    if (res > -1)
            //    {
            //        result = true;
            //    }
            //    else
            //    {
            //        result = false;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    result = false;
               
            //}
            //finally
            //{
            //    _application.DbInfo.Connection.Close();
            //}
            return false;
           
        }

        public bool CheckDeviceState(dloDataApplication application)
        {
            _application = application;
            bool result = false;
            try
            {
                DbCommand cmd = null;
                cmd = _application.DbInfo.Connection.CreateCommand();
                var provider = _application.Provider;
                int count = 0;
                switch (_application.Provider)
                {
                    case DataProviders.SQL:
                    case DataProviders.SQLite:
                        cmd.CommandText = string.Format("SELECT COUNT(*) FROM dsto_devices WHERE IMEI ='{0}' AND Status = 1", Imei);
                        if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                            _application.DbInfo.Connection.Open();
                        count = (int)_application.DbInfo.ExecuteScalar(cmd);
                        result = count > 0;                  
                        break;
                    case DataProviders.MYSQL:
                        cmd.CommandText = string.Format("SELECT COUNT(*) FROM dsto_devices WHERE IMEI ='{0}' AND Status = 1", Imei);
                        if (_application.DbInfo.Connection.State == System.Data.ConnectionState.Closed)
                            _application.DbInfo.Connection.Open();
                        count = (int)_application.DbInfo.ExecuteScalar(cmd);
                        result = count > 0;
                        break;
                }
            }
            catch(Exception ex)
            {
                result = false;
                
            }
            finally
            {
                _application.DbInfo.Connection.Close();
            }
            return result;
        }
    }
}
