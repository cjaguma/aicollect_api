using AiCollect.Core;

//using Datalabs.Sync;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloDataApplication : IServiceProvider
    {
        #region Events
        public event LanguageChangedHandler LanguageChanged;
        public event DatabaseQueryExcecutingHandler Upgrading;
        #endregion

        #region Properties
        protected ServiceContainer ServiceContainer { get; private set; }
        public string Key
        {
            get
            {
                return Configuration.Key;
            }
        }

        protected bool IsCached { get; set; }

        //public dloDataSet DataSet { get; internal set; }
        public dloDbInfo DbInfo { get; internal set; }

        public bool IsLoaded { get; internal set; }

        //public dloUser CurrentUser { get; internal set; }

        //private dloDevice _currentDevice;
        //public dloDevice CurrentDevice
        //{
        //    get
        //    {
        //        if (!_currentDevice.IsLoaded)
        //            _currentDevice.Load();
        //        return _currentDevice;
        //    }
        //}

        private Configuration _configuration;
        public Configuration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        private DataProviders _provider;
        public DataProviders Provider
        {
            get
            {
                return _provider;
            }
        }
        /// <summary>
        /// Gets  DatsList available in the configuration
        /// </summary>
        public EnumLists Lists { get { return _configuration.EnumerationLists; } }

        /// <summary>
        /// Gets the status of synchronisation operation
        /// </summary>
        public SyncStatus SyncStatus { get; internal set; }

        public DateTime LastSync { get; internal set; }

        public bool CanSync
        {
            get
            {
                if (_configuration.SyncDirection != SyncDirection.None)
                    return true;
                else
                    return false;
            }
        }

        //public RuntimeAuthTypes AuthTypes { get; protected set; }
        //private Session _session;
        //public Session Session
        //{
        //    get
        //    {

        //        return _session;
        //    }
        //    private set
        //    {
        //        _session = value;
        //    }
        //}

        public bool IsAuthenticated { get; private set; }

        
        public DatabaseQueries Queries { get; set; }
        public bool _isImported;
        
        #endregion

        #region Constructors
        public dloDataApplication()
        {
            //Create a new instance of our service container
            ServiceContainer = new System.ComponentModel.Design.ServiceContainer();

            IsLoaded = false;

            //  AuthTypes = RuntimeAuthTypes.None;

            SyncStatus = Data.SyncStatus.Unknown;

            DbInfo = new dloDbInfo(this);

            Configuration = new Configuration();

            

            Queries = new DatabaseQueries();
            //CurrentUser = new dloUser(_users);

            //_currentDevice = new dloDevice(this);

            //DoHouseKeeping();

            //Register services


        }

        #endregion

        #region ServiceContainer implementation
        public void AddService(Type type, object obj)
        {
            if (ServiceContainer != null)
                ServiceContainer.AddService(type, obj);
        }

        public object GetService(Type serviceType)
        {
            if (ServiceContainer != null)
                return ServiceContainer.GetService(serviceType);
            else
                return null;
        }

        public void RemoveService()
        {

        }
        #endregion


        #region  Indexers
        //public dloDataTable this[string name]
        //{
        //    get
        //    {
        //        return this.DataSet.DataTables.SingleOrDefault(t => t.DataObject.Name.Equals(name));
        //    }
        //}
        #endregion

        public void OnLanguagedChanged()
        {
            if (LanguageChanged != null)
                LanguageChanged();
        }
        //private void DoHouseKeeping()
        //{
        //    //Check if the cache folder exists if not create it
        //    if (!Directory.Exists((Path.Combine(System.Windows.Forms.Application.StartupPath, "Cache"))))
        //        Directory.CreateDirectory(Path.Combine(System.Windows.Forms.Application.StartupPath, "Cache"));

        //    //make sure we have a stores directory
        //    if (!Directory.Exists(Path.Combine(System.Windows.Forms.Application.StartupPath, "Cache", "Stores")))
        //        Directory.CreateDirectory(Path.Combine(System.Windows.Forms.Application.StartupPath, "Cache", "Stores"));
        //}


        //public virtual void Load(Session session, Configuration configuration)
        //{
        //    Session = session;
        //    Load(Session.User.UserName, Session.User.Password, configuration);
        //}

        public void Load(string username, string password, Configuration configuration, bool preLoadSchema = true)
        {
            //CurrentUser.Username = username;
            //CurrentUser.Password = password;

            //Configuration = configuration;

            //ConnectionSettings settings = Configuration.DataStores.Default.Settings;

            //_provider = settings.Provider;



            //        DbInfo.ConnectionString = settings.ConnectionString;
            //        DbInfo.MasterConnectionString = settings.MasterConnectionString;
            //break;


            DbInfo.Provider = _provider;



            //initialise our data objects
            if (preLoadSchema)
                Refresh();

            //if ((Session != null && !Session.IsAuthenticated) || Session == null)
            //{

            //    if (AuthTypes == RuntimeAuthTypes.None)
            //    {
            //        //Attempt to login using the runtime authetication modes specified in the configuration
            //        if (Session == null ||  CurrentUser.IsChanged)
            //            Session = new ApplicationSession(this, configuration.AuthTypes, CurrentUser);
            //    }
            //    else
            //    {
            //        if (Session == null)
            //            Session = new ApplicationSession(this, AuthTypes, CurrentUser);
            //    }

            //    IsAuthenticated = ((ApplicationSession)Session).Authenticate(false);

            //    if (!IsAuthenticated)
            //    {
            //        LoggingService.WriteLog(3, "AUTH300", string.Format("Failed login attempt by {0}", CurrentUser.Username));
            //        throw new Exception("Authentication failed");
            //    }
            //}

        }

        public void Load(string username, string password, DataProviders provider, string connectionString)
        {

            _provider = provider;

            //CurrentUser.Username = username;
            //CurrentUser.Password = password;

            //DbInfo.ConnectionString = connectionString;
            DbInfo.Provider = _provider;

            Init();
        }

        private void Init()
        {
            //ProgressReporter.Report("Reading from cache");

            //_configuration = ImportConfigurationFromCache();

            //ProgressReporter.Report("Initialising data objects");
            //initialise our data objects
            Refresh();
        }

        /// <summary>
        /// Imports configuration from cached datastore using the current settings
        /// </summary>
        protected Configuration Import()
        {
            Configuration config = null;
            CachedConfiguration cachedConfig = DbInfo.Import();

            config = new Configuration();
            //config.Load(new StringReader(cachedConfig.Xml));

            return config;
        }

        protected Configuration Import(CachedConfiguration cachedConfig)
        {
            Configuration config = new Configuration();
            //config.Load(new StringReader(cachedConfig.Xml));
            return config;
        }

       

      
        /// <summary>
        /// Upgrades the configuration in the database to reflect new changes
        /// </summary>
        /// <param name="item"></param>
        public DatabaseQueries Upgrade(Configuration config)
        {
            return GenerateDatabase(false, config);
        }

        public void GenerateDatabase(Configuration config)
        {
            Configuration = config;

            // ConnectionSettings settings = Configuration.DataStores.Default.Settings;

            _provider = config.DbInfo.DatabaseType;

            switch (_provider)
            {
                case DataProviders.SQLite:
                    //settings.FullPath = settings.Path;
                    //DbInfo.ConnectionString = settings.ConnectionString;
                    //DbInfo.MasterConnectionString = settings.MasterConnectionString;
                    break;
                default:
                    //DbInfo.ConnectionString = settings.ConnectionString;
                    //DbInfo.MasterConnectionString = settings.MasterConnectionString;

                    //DbInfo.ConnectionString = config.DbInfo.ConnectionString;
                    //DbInfo.MasterConnectionString = config.DbInfo.MasterConnectionString;
                    break;
            }

            DbInfo.Provider = _provider;
            DbInfo.Database = Configuration.DbInfo.Database;
            DbInfo.Server = Configuration.DbInfo.Server;
            DbInfo.Authentication = Configuration.DbInfo.Authentication;
            DbInfo.User = Configuration.DbInfo.User;
            DbInfo.Password = Configuration.DbInfo.Password;
            var exists = DbInfo.DatabaseExists();
            this.Queries.Append(GenerateDatabase(exists, Configuration));
        }

        public DatabaseQueries GenerateDatabase(bool exists, Configuration configuration)
        {

            DatabaseQueries dbQueries = new DatabaseQueries();

            if (!exists)
            {

                //Create  a new database and system tables
                dbQueries.Append(Configuration.CreateNewDatabase(Provider));

                dbQueries.Append(Configuration.CreateSystemTables(Provider));

                //Save the configuration
                dbQueries.Append(Configuration.CreateSaveConfiguration(Provider));

                //Create data object tables
                //dbQueries.Append(Configuration.Questionaires.GenerateDatabase(Provider));

                //Create enumeration tables
                dbQueries.Append(Configuration.EnumerationLists.GenerateDatabase(Provider));

                //Create stored procedures
              //  dbQueries.Append(Configuration.Procedures.GenerateDatabase(Provider));

                //creating junction tables
                dbQueries.Append(Configuration.GenerateJunctionTables());
             
                //do inserts
                foreach(var questionaire in Configuration.Questionaires)
                {
                    dbQueries.Append(questionaire.Insert());
                }


            }
            else
            {
                //Save the configuration
                dbQueries.Append(configuration.CreateUpdateConfiguration(Provider));

                ////update the data object tables
                dbQueries.Append(configuration.Questionaires.GenerateDatabase(Provider, Configuration.Questionaires));

                ////update enumeration tables
                dbQueries.Append(configuration.EnumerationLists.GenerateDatabase(Provider, Configuration.EnumerationLists));              

            }

            return dbQueries;
        }

        /// <summary>
        /// Checks to see if the Default Data Store already exists
        /// </summary>
        /// <returns></returns>
        protected bool DataStoreExists()
        {
            switch (Provider)
            {
                case DataProviders.MYSQL:
                case DataProviders.Oracle:
                case DataProviders.SQL:
                    break;
                case DataProviders.SQLite:
                    return DbInfo.DatabaseExists(Configuration.Key);
                default:
                    throw new Exception("Unsupported data provider");
            }
            return false;

        }

        

        private void OnUpgrading(DatabaseQuery q)
        {
            if (Upgrading != null)
                Upgrading(q);
        }

        public bool ExecuteQueries()
        {
            ////Execute the queries
            foreach (DatabaseQuery q in Queries)
            {
                if (q.Message == Messages.DropDatabase || q.Message == Messages.CreateDatabase || q.Message == Messages.EnableDBCT || q.Message == Messages.DisableDBCT || q.Message == Messages.AlterDatabase)
                {

                    OnUpgrading(q);
                    string error = "";
                    int queryResult = DbInfo.ExecuteNonQuery(q.SqlStatement, true, out error);

                    if (queryResult > 0 || queryResult == -1)
                    {
                        q.Result = 1;

                    }
                    else
                    {
                        q.Result = -1;
                        q.ErrorMessage = error;
                    }
                }
            }

            //// Execute all sql statements NOT related to create, drop or alter database etc...
            DbInfo.BeginTransaction(false);
            foreach (DatabaseQuery q in Queries)
            {
                if (q.Message != Messages.DropDatabase && q.Message != Messages.CreateDatabase && q.Message != Messages.EnableDBCT && q.Message != Messages.DisableDBCT && q.Message != Messages.AlterDatabase)
                {

                    OnUpgrading(q);
                    string error = "";
                    int queryResult = DbInfo.ExecuteNonQuery(q.SqlStatement, out error);

                    if (queryResult > 0 || queryResult == -1)
                    {
                        q.Result = 1;

                    }
                    else
                    {
                        q.Result = -1;
                        q.ErrorMessage = error;
                    }
                }
            }

            //// Rollback transaction if execution of any sql statement failed on the database. Otherwise commit transaction
            if (Queries.HasErrors)
            {
                DbInfo.RollBackTransaction();
                return false;
            }
            else
            {
                DbInfo.CommitTransaction();
                return true;
            }

        }


        public virtual string Translate(string str)
        {
            return str;
        }

        /// <summary>
        /// Instantiate a new DataSet object and let it (re)load all configurated items
        /// </summary>
        public void Refresh()
        {
            Refresh(false);
        }

        public void Refresh(bool dataOnly)
        {
            //if (dataOnly)
            //{
            //    DataSet.Refresh();
            //}
            //else
            //{
            //    DataSet = new dloDataSet(this);
            //}
        }

        public virtual void Update()
        {
            // DataSet.Update();
        }

        #region Sync implementations


        //from the from the server to the client
        //public async Task<dsoDataSet> DownSync()
        //{
        //    string url = string.Format("/CloudSync/download?os=Windows&userkey={0}", CurrentUser.Id);
        //    if (this.CanSync)
        //    {
        //        dsoDataSet dsoStructure = DataSet.TodsoDataSetStructure();
        //        IOutputService output = (IOutputService)GetService(typeof(IOutputService));
        //        SyncResponse response = null;

        //        SyncRequest req = new SyncRequest(dsoStructure, url);

        //        response = await req.Execute();

        //        if (response.Status == ResponseStatuses.Success)
        //        {
        //            dsoStructure = response.DataSet;

        //            //START HERE TO PROVIDE LOGIC TO INSERT,DELETE,UPDATE TO LOCAL CACHED DATABASE
        //            DownLoadToLocalDb(dsoStructure);

        //            //SEND BACK DSOdATASET TO CONFIRM LOG ERRORS CONFLICTS THT HAPENED THRU DOWNLOAD OPERATION
        //            //call the conflict controller here


        //            return dsoStructure;
        //        }
        //        else
        //        {

        //            if (output != null)
        //                output.Append(response.Message, OutputMessageTypes.Error);

        //        }
        //    }

        //    return null;
        //}

        //private void UpLoadConflictErrors(dsoDataSet dset)
        //{

        //    string url = string.Format("http://localhost/DatalabsWeb/api/conflicts?os={0}&projectid={1}&userkey={2}", Environment.OSVersion.VersionString, Key, CurrentUser.Id);
        //    string dsoString = dset.ToJsonString();
        //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        //    request.Credentials = new NetworkCredential(CurrentUser.Username, CurrentUser.Password);

        //    request.Method = "Post";
        //    request.ContentType = "application/json";
        //    request.ContentLength = dsoString.Length;

        //    Stream dataStream = request.GetRequestStream();
        //    StreamWriter requestWriter = new StreamWriter(dataStream);
        //    requestWriter.Write(dsoString);
        //    requestWriter.Close();

        //    var response = (HttpWebResponse)request.GetResponse();
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        string returnValue = null;
        //        Stream stream;
        //        using (var responseStream = response.GetResponseStream())
        //        {

        //            stream = responseStream;
        //            using (var reader = new StreamReader(responseStream))
        //            {
        //                returnValue = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //}

        //private void DownLoadToLocalDb(dsoDataSet dset)
        //{

        //    List<dsoDbCommand> cmds = new List<dsoDbCommand>();
        //    dsoDbCommand cmd = null;
        //    foreach (var table in dset.Tables)
        //    {

        //        foreach (dsoDataRow serverRow in table.Rows)
        //        {
        //            ColumnValueWrapper serverCO = serverRow.ByName("ChangeOperation");
        //            ColumnValueWrapper serverGuid = serverRow.ByName("Guid");
        //            if (serverCO.Data.ToString().Equals(ChangeOperationState.I.ToString()))
        //            {
        //                //check if the record is realy new
        //                //INSERT UPDATE LOGIC cos the server has more powers over the local values
        //                //if not exists insert and if exists update
        //                cmd = new dsoDbCommand(this.DbInfo);
        //                cmd.CreateInsertCommand(serverRow, table);
        //                cmds.Add(cmd);
        //            }
        //            else if (serverCO.Data.ToString() == ChangeOperationState.U.ToString())
        //            {
        //                string localChangeOperation = CheckLocalChangeOperation(string.Format("SELECT ChangeOperation FROM {0} WHERE GUID='{1}'", table.TableName, serverGuid.Data)) as string;
        //                if (localChangeOperation == ChangeOperationState.N.ToString())
        //                {
        //                    //safe to update record
        //                    cmd = new dsoDbCommand(this.DbInfo);
        //                    cmd.CreateUpdateCommand(serverRow, table);
        //                    cmds.Add(cmd);
        //                }
        //                else if (localChangeOperation == ChangeOperationState.U.ToString())
        //                {
        //                    serverRow.SyncResult = SyncResult.Conflict;
        //                    serverRow.RowError = "LocalUpdateRemoteUpdate";
        //                }
        //                else if (localChangeOperation == ChangeOperationState.D.ToString())
        //                {
        //                    serverRow.SyncResult = SyncResult.Conflict;
        //                    serverRow.RowError = "LocalDeleteRemoteUpdate";
        //                }

        //            }
        //            else if (serverCO.Data.ToString() == ChangeOperationState.D.ToString())
        //            {
        //                //LocalUpdateRemoteDelete
        //                string localChangeOperation = CheckLocalChangeOperation(string.Format("SELECT ChangeOperation FROM {0} WHERE GUID='{1}'", table.TableName, serverGuid.Data)) as string;
        //                if (localChangeOperation == ChangeOperationState.N.ToString() || localChangeOperation == ChangeOperationState.D.ToString())
        //                {
        //                    //safe to delete record
        //                    cmd = new dsoDbCommand(this.DbInfo);
        //                    cmd.CreateDeleteCommand(serverRow, table);
        //                    cmds.Add(cmd);
        //                }
        //                else if (localChangeOperation == ChangeOperationState.U.ToString())
        //                {
        //                    serverRow.SyncResult = SyncResult.Conflict;
        //                    serverRow.RowError = "LocalUpdateRemoteDelete";
        //                }
        //            }

        //        }

        //        //Adjust dsodataset with conflict errors
        //        FlagConflictedRows(dset);

        //        //provide excute dsodbcomands here
        //        ExcecuteCommands(cmds);

        //    }

        //}

        //private void ExcecuteCommands(List<dsoDbCommand> cmds)
        //{
        //    IOutputService output = (IOutputService)GetService(typeof(IOutputService));
        //    foreach (var item in cmds)
        //    {
        //        try
        //        {
        //            int result = 0;
        //            if (item.Result == DsoCommandAction.Insert)
        //            {
        //                ColumnValueWrapper guidvalue = item.Row.ColumnValues.SingleOrDefault(cv => cv.ColumnName == "Guid");

        //                string q = string.Format("select count(GUID) from {0} where [GUID]='{1}'", item.Row.Table.TableName, guidvalue.Data);
        //                var cmd = new dsoDbCommand(this.DbInfo);
        //                cmd.CreateCommand(q);
        //                result = (int)this.DbInfo.ExecuteScalar(cmd.command);
        //                if (result == 0)
        //                {
        //                    var res = this.DbInfo.ExecuteCECommand(item.command);
        //                    if (res != -2)
        //                        item.Row.SyncResult = SyncResult.Success;
        //                    else
        //                    {
        //                        item.Row.SyncResult = SyncResult.Failed;

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var res = this.DbInfo.ExecuteCECommand(item.command);
        //                if (res != -2)
        //                    item.Row.SyncResult = SyncResult.Success;
        //                else
        //                {
        //                    item.Row.SyncResult = SyncResult.Failed;

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //log error back to the main server thru the web api as a conflict error
        //            item.Row.SyncResult = SyncResult.Failed;
        //            item.Row.RowError = ex.Message;
        //            if (output != null)
        //                output.Append("Sync Error: " + ex.Message, OutputMessageTypes.Error);
        //        }
        //    }

        //}

        //private void FlagConflictedRows(dsoDataSet Dset)
        //{
        //    //FLAG LOCAL RECORD
        //    //conflict, not safe to update record instead flag record
        //    //NB: here we replace the serverow to the local row since the conflict will be resolved at the desktop level

        //    if (DbInfo.Connection.State == System.Data.ConnectionState.Closed)
        //        DbInfo.Connection.Open();

        //    foreach (dsoDataTable table in Dset.Tables)
        //    {
        //        foreach (dsoDataRow row in table.Rows)
        //        {
        //            if (row.SyncResult == SyncResult.Conflict)
        //            {
        //                AdjustAndFlagServerRowWithLocalRowValues(row, DbInfo.Connection);
        //            }
        //        }
        //    }

        //    DbInfo.Connection.Close();

        //}

        private object CheckLocalChangeOperation(string querry)
        {
            object obj = null;
            using (DbConnection con = this.DbInfo.Connection)
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandText = querry;
                con.Open();
                obj = cmd.ExecuteScalar();
            }

            return obj;
        }

        //private void AdjustAndFlagServerRowWithLocalRowValues(dsoDataRow row, DbConnection con)
        //{
        //    //comand for getting local changes
        //    DbCommand cmd = con.CreateCommand();
        //    ColumnValueWrapper cvr = row.ByName("Guid");
        //    cmd.CommandText = string.Format("SELECT * FROM {0} WHERE GUID='{1}'", row.Table.TableName, cvr.Data);

        //    using (DbDataReader reader = cmd.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    ColumnValueWrapper wrp = row.ByName(reader.GetName(i));
        //                    if (wrp != null)
        //                    {
        //                        if (!wrp.ColumnName.Equals("Guid"))
        //                        {
        //                            wrp.Data = reader[i];
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //Flag record after words
        //    DbCommand cmdFlagRecord = con.CreateCommand();
        //    cmdFlagRecord.CommandText = string.Format("UPDATE {0} SET ChangeOperation='{1}' WHERE GUID='{2}'", row.Table.TableName, ChangeOperationState.C.ToString(), cvr.Data);
        //    cmdFlagRecord.ExecuteNonQuery();
        //}

        //public async void TwoWaySync(dsoDataSet dataSet, SyncParameters param, bool c = true)
        //{
        //    await UpSync();
        //    await DownSync();
        //}

        ////from the client to the server
        //public async Task<dsoDataSet> UpSync()
        //{
        //    string url = string.Format("/CloudSync/Upload?os=Windows&userkey={0}", CurrentUser.Id);
        //    if (this.CanSync)
        //    {
        //        SyncStatus = Data.SyncStatus.Syncing;
        //        dsoDataSet returnedDataset = null;// stream as dsoDataSet;
        //        IOutputService output = (IOutputService)GetService(typeof(IOutputService));
        //        dsoDataSet syncDataSet = this.DataSet.TodsoDataSet();

        //        SyncRequest req = new SyncRequest(syncDataSet, url);
        //        SyncResponse response = await req.Execute();
        //        if (response.Status == ResponseStatuses.Success)
        //        {
        //            returnedDataset = response.DataSet;
        //        }
        //        else
        //        {

        //            if (output != null)
        //                output.Append(response.Message, OutputMessageTypes.Error);

        //            throw new Exception("Upload failed! Don't worry still checking out some stuff");
        //        }

        //        SyncStatus = Data.SyncStatus.Completed;
        //        return returnedDataset;
        //    }
        //    else
        //    {
        //        SyncStatus = Data.SyncStatus.Failed;
        //        return null;
        //    }

        //}

        //private object UpdateValues(dsoDataSet dsodataset)
        //{

        //    foreach (dsoDataTable dsotable in dsodataset.Tables)
        //    {
        //        List<string> querries = new List<string>();

        //        foreach (dsoDataRow dsorow in dsotable.Rows)
        //        {

        //            ColumnValueWrapper wrp = dsorow.ByName("Guid");
        //            string querry;
        //            switch (dsorow.SyncResult)
        //            {
        //                case SyncResult.Unknown:
        //                    break;
        //                case SyncResult.Success:
        //                    //update the co to N
        //                    querry = string.Format("UPDATE {0} SET ChangeOperation='{1}' WHERE Guid='{2}';", dsotable.TableName, ChangeOperationState.N, wrp.Data);
        //                    querries.Add(querry);
        //                    break;
        //                case SyncResult.Failed:
        //                    //update the co with E
        //                    querry = string.Format("UPDATE {0} SET ChangeOperation='{1}' WHERE Guid='{2}';", dsotable.TableName, ChangeOperationState.E, wrp.Data);
        //                    querries.Add(querry);
        //                    break;
        //                case SyncResult.Conflict:
        //                    //Update the co with C
        //                    querry = string.Format("UPDATE {0} SET ChangeOperation='{1}' WHERE Guid='{2}';", dsotable.TableName, ChangeOperationState.C, wrp.Data);
        //                    querries.Add(querry);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        ConfirmUpdate(querries);
        //    }


        //    return dsodataset;
        //}

        private void ConfirmUpdate(List<string> queries)
        {
            foreach (string item in queries)
            {
                this.DbInfo.ExecuteQuery(item);
            }
        }

        //Get available conflicts

        //protected List<dsoConflict> LoadConflicts(string url)
        //{
        //    try
        //    {
        //        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        //        request.Credentials = new NetworkCredential(CurrentUser.Username, CurrentUser.Password);

        //        request.Method = "Get";
        //        request.ContentType = "application/json";

        //        var response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK)
        //        {
        //            string returnValue = null;
        //            Stream stream;
        //            using (var responseStream = response.GetResponseStream())
        //            {

        //                stream = responseStream;
        //                using (var reader = new StreamReader(responseStream))
        //                {
        //                    returnValue = reader.ReadToEnd();
        //                }
        //            }

        //            JsonSerializerSettings jsettings = new JsonSerializerSettings();
        //            jsettings.NullValueHandling = NullValueHandling.Ignore;
        //            List<dsoConflict> conflicts = (List<dsoConflict>)JsonConvert.DeserializeObject(returnValue, typeof(List<dsoConflict>), jsettings);
        //            return conflicts;
        //        }
        //        return null;
        //    }
        //    catch
        //    {

        //        return null;
        //    }

        //}
        #endregion Sync implementations


    }

    public class CachedConfiguration
    {
        public string Key { get; set; }
        public string Version { get; set; }
        public string Xml { get; set; }
        public string FormsDll { get; set; }
        public string ScriptsDll { get; set; }
    }


    public class SyncParameters
    {
        public bool IsFiltered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ChangeOperations Operations { get; set; }

        public SyncParameters()
        {
            Operations = ChangeOperations.Insert | ChangeOperations.Update | ChangeOperations.Delete;
        }
    }

    public enum SyncStatus
    {
        Unknown,
        Current,
        Syncing,
        Pending,
        Completed,
        Failed
    }

    [Flags]
    public enum ChangeOperations
    {
        Insert,
        Update,
        Delete
    }


    public delegate void LanguageChangedHandler();


}
