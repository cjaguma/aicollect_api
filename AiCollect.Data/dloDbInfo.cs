using AiCollect.Core;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloDbInfo
    {
        private DbTransaction _sqlTrans;


        internal bool IsTransactionActive
        {
            get
            {
                return _sqlTrans != null ? true : false;
            }
        }

        private DbConnection _connection;
        public DbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = CreateConnection();
                }
                return _connection;
            }
        }

        private DbConnection _masterConnection;
        public DbConnection MasterConnection
        {
            get
            {
                if (_masterConnection == null)
                {
                    _masterConnection = CreateConnection(true);
                }
                return _masterConnection;
            }
        }
        private string _server;

        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                if (value != _server)
                {
                    _server = value;
                }
            }
        }

        private Authentications _authentication;

        public Authentications Authentication
        {
            get
            {
                return _authentication;
            }
            set
            {
                if (value != _authentication)
                {
                    _authentication = value;
                }
            }
        }

        private string _user;

        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (value != _user)
                {
                    _user = value;
                }
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                }
            }
        }

        private string _database;

        public string Database
        {
            get
            {
                return _database;
            }
            set
            {
                if (value != _database)
                {
                    _database = value;
                }
            }
        }

        private DatabaseTypes _databaseType;

        public DatabaseTypes DatabaseType
        {
            get { return _databaseType; }
            set
            {
                if (value != _databaseType)
                {
                    _databaseType = value;
                }
            }
        }

        public DataProviders Provider { get; set; }

        private DbConnection CreateConnection(bool master = false)
        {
            switch (Provider)
            {
                case DataProviders.POSTGRESSQL:
                    return new NpgsqlConnection(master ? MasterConnectionString : ConnectionString);
                case DataProviders.SQL:
                    return new SqlConnection(master ? MasterConnectionString : ConnectionString);
                case DataProviders.MYSQL:
                    return new MySqlConnection(master ? MasterConnectionString : ConnectionString);
                case DataProviders.SQLite:
                    return new SQLiteConnection(master ? MasterConnectionString : ConnectionString);

            }
            return null;
        }

        private DbCommand CreateCommand()
        {
            DbCommand cmd = null;
            switch (Provider)
            {
                case DataProviders.POSTGRESSQL:
                    cmd = new NpgsqlCommand();
                    break;
                case DataProviders.SQL:
                    cmd = new SqlCommand();
                    break;
                case DataProviders.MYSQL:
                    cmd = new MySqlCommand();
                    break;
                case DataProviders.SQLite:
                    cmd = new SQLiteCommand();
                    break;
            }

            return cmd;
        }

        internal DbCommand CreateSqlCommand()
        {
            DbCommand cmd = null;
            cmd = CreateCommand();
            if (cmd != null)
                cmd.Connection = Connection;

            return cmd;
        }

        internal DbCommand CreateDbCommand()
        {
            DbCommand cmd = CreateCommand();
            if (cmd != null)
                cmd.Connection = Connection;

            return cmd;
        }

        internal DbCommand CreateSqlCommand(bool withTransaction)
        {
            if (withTransaction)
            {
                if (_sqlTrans == null)
                {
                    throw new Exception("Cannot create an SqlCommand without an active transaction");
                }
                var cmd = _sqlTrans.Connection.CreateCommand();
                cmd.Transaction = _sqlTrans;

                return cmd;
            }

            // Return a non-transactional SqlCommand
            var command = CreateCommand();
            command.Connection = Connection;
            return command;
        }

        private string _connectionString;
        public string ConnectionString
        {
            get;
            set;
        }

        private string _masterConnectionString;
        public string MasterConnectionString
        {
            get
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("Server=");
                sb.Append(Server);
                sb.Append(";Database=Master");
                if (Authentication == Authentications.Windows)
                {
                    sb.Append(";Trusted_Connection=True;");
                }
                else
                {
                    sb.Append(";User ID=");
                    sb.Append(User);
                    sb.Append(";Password=");
                    sb.Append(Password);
                    sb.Append(";");
                }
                //Pooling
                sb.Append("Pooling=false;");
                return sb.ToString();
            }

            set
            {

            }
        }

        public void GetValues(string val)
        {
            Database = GetDatabase(val);
            Server = GetServer(val);
            User = GetUser(val);
            Password = GetPassword(val);
        }

        public string GetDatabase(string connectionString)
        {
            string[] parts = connectionString.Split(';');
            foreach (var p in parts)
            {
                if (!p.StartsWith("Initial Catalog")) continue;
                string[] dbString = p.Split('=');
                return dbString[1];

            }
            return "";
        }
        public string GetServer(string connectionString)
        {
            string[] parts = connectionString.Split(';');
            foreach (var p in parts)
            {
                if (!p.StartsWith("Data Source")) continue;
                string[] dbString = p.Split('=');
                return dbString[1];

            }
            return "";
        }
        public string GetUser(string connectionString)
        {
            string[] parts = connectionString.Split(';');
            foreach (var p in parts)
            {
                if (!p.StartsWith("User Id")) continue;
                string[] dbString = p.Split('=');
                return dbString[1];

            }
            return "";
        }

        public string GetPassword(string connectionString)
        {
            string[] parts = connectionString.Split(';');
            foreach (var p in parts)
            {
                if (!p.StartsWith("Password")) continue;
                string[] dbString = p.Split('=');
                return dbString[1];

            }
            return "";
        }
        public dloDbInfo() { }
        public dloDbInfo(dloDataApplication app)
        {
            //Application = app;
        }

        public void BeginTransaction(bool master = false)
        {
            if (_connection == null)
            {
                _connection = CreateConnection(master);
            }
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
            if (_sqlTrans == null)
                _sqlTrans = Connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_sqlTrans != null)
            {
                try
                {
                    _sqlTrans.Commit();
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    _sqlTrans = null;
                }
                catch
                {

                }
                finally
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    _sqlTrans = null;
                }
            }
        }

        /// <summary>
        /// Rolls back an failed transaction
        /// </summary>
        public void RollBackTransaction()
        {
            if (_sqlTrans != null)
                _sqlTrans.Rollback();
            Connection.Close();
            _sqlTrans = null;
        }

        /// <summary>
        /// Creates the native Data provider's DataAdapter
        /// </summary>
        /// <returns></returns>
        private DbDataAdapter CreateAdapter()
        {
            DbDataAdapter adapter = null;
            switch (Provider)
            {
                case DataProviders.POSTGRESSQL:
                    adapter = new NpgsqlDataAdapter();
                    break;
                case DataProviders.SQL:
                    adapter = new SqlDataAdapter();
                    break;
                case DataProviders.MYSQL:
                    adapter = new MySqlDataAdapter();
                    break;

                case DataProviders.SQLite:
                    adapter = new SQLiteDataAdapter();
                    break;
            }

            return adapter;
        }



        public string SqliteDatabaseFileName { get; set; }
        public int MaxDependencies { get; set; }

        /// <summary>
        /// Creates a SQL Compact database
        /// </summary>
        public void CreateSqliteDatabase()
        {
            if (string.IsNullOrWhiteSpace(SqliteDatabaseFileName))
                throw new Exception("Database file name is empty");
            //create the database
            SQLiteConnection.CreateFile(SqliteDatabaseFileName);
        }

        public int ExecuteQuery(string query, System.Data.DataTable datatable, bool master = false, bool withTransaction = false)
        {

            int ret = 0;
            DbCommand cmd;
            DbDataAdapter adapter;
            DbConnection conn = null;

            if (master)
                conn = MasterConnection;
            else
                conn = Connection;

            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            cmd = CreateSqlCommand(false);
            cmd.Transaction = withTransaction ? _sqlTrans : null;
            adapter = CreateAdapter();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            ret = adapter.Fill(datatable);

            return ret;
        }


        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, false);
        }

        public int ExecuteNonQuery(string query, out string errorMessage)
        {
            return ExecuteNonQuery(query, false, out errorMessage);
        }

        public DataTable ExecuteSelectQuery(string query)
        {
            DbConnection conn = Connection;
            DbCommand sqlCmd = conn.CreateCommand();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = query;
            DataTable table = new DataTable();
            try
            {
                if (sqlCmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlCmd.Connection.Open();
                DbDataAdapter adapter = CreateAdapter();
                adapter.SelectCommand = sqlCmd;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                return table;
            }
            finally
            {

            }
        }

        internal int ExecuteQuery(string query)
        {
            DbConnection conn = Connection;
            DbCommand sqlCmd = conn.CreateCommand();
            sqlCmd.Connection = conn;
            sqlCmd.CommandText = query;

            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                return sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                return -2;
            }
            finally
            {
                conn.Close();
            }
        }

        internal int ExecuteCommand(DbCommand sqlCmd, ref DataTable table)
        {

            try
            {
                if (sqlCmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlCmd.Connection.Open();
                DbDataAdapter adapter = CreateAdapter();
                adapter.SelectCommand = sqlCmd;
                int ret = adapter.Fill(table);

                return ret;
            }
            catch (Exception ex)
            {

                return -2;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }
        }

        internal int ExecuteCECommand(DbCommand sqlCmd)
        {

            try
            {
                if (sqlCmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlCmd.Connection.Open();
                return sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                return -2;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }
        }

        public object ExecuteScalar(DbCommand sqlCmd)
        {

            try
            {
                if (sqlCmd.Connection.State != System.Data.ConnectionState.Open)
                    sqlCmd.Connection.Open();
                return sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return null;
            }
            finally
            {
                sqlCmd.Connection.Close();
            }
        }

        public int ExecuteNonQuery(string query, bool master)
        {
            string err = "";
            return ExecuteNonQuery(query, master, out err);
        }

        public int ExecuteNonQuery(string query, bool master, out string errorMessage, bool withTransaction = false)
        {
            int ret = 0;
            //check if there is a transaction
            DbCommand sqlCmd;
            DbConnection conn;
            errorMessage = "";

            if (_sqlTrans != null && (_sqlTrans.Connection != null))
            {
                conn = _sqlTrans.Connection;
                sqlCmd = conn.CreateCommand();
                sqlCmd.Connection = conn;
                sqlCmd.Transaction = withTransaction ? _sqlTrans : null;
                sqlCmd.CommandText = query;
                if (Provider != DataProviders.SQLCE)
                    sqlCmd.CommandTimeout = 6000;
                try
                {
                    ret = sqlCmd.ExecuteNonQuery();
                    return ret;
                }
                catch (Exception ex)
                {
                    return -2;
                }
            }
            else
            {
                if (master == true)
                    conn = MasterConnection;
                else
                    conn = Connection;

                if (conn == null)
                    return -2;

                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                sqlCmd = CreateCommand();
                sqlCmd.CommandText = query;
                sqlCmd.Connection = conn;
                //sqlCmd.CommandTimeout = 300;
                try
                {
                    ret = sqlCmd.ExecuteNonQuery();
                    return ret;
                }
                catch (Exception ex)
                {
                    return -2;
                }

            }


        }


        /// <summary>
        /// Imports a cached configuration
        /// </summary>
        /// <returns></returns>
        public CachedConfiguration Import()
        {
            CachedConfiguration c = new CachedConfiguration();
            string strReadSQL = "SELECT top 1 * FROM [dsto_configuration] order by date_created DESC";

            switch (Provider)
            {
                case DataProviders.POSTGRESSQL:
                    strReadSQL = "SELECT * FROM [dsto_configuration] ORDER BY date_created DESC LIMIT 0,1";
                    break;
                case DataProviders.SQL:
                    strReadSQL = "SELECT top 1 * FROM [dsto_configuration] ORDER BY date_created DESC";
                    break;
                case DataProviders.MYSQL:
                    strReadSQL = "SELECT * FROM [dsto_configuration] ORDER BY date_created DESC LIMIT 0,1";
                    break;
                case DataProviders.SQLite:
                    strReadSQL = "SELECT * FROM [dsto_configuration] ORDER BY date_created DESC LIMIT 0,1";
                    break;
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            if (ExecuteQuery(strReadSQL, dt) > 0)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    c.Version = Convert.ToString(dr["Version"]);
                    c.Xml = Convert.ToString(dr["Config"]);
                    c.Key = Convert.ToString(dr["Key"]);

                }
            }
            return c;
        }

        public bool DatabaseExists(string database = null)
        {
            string query = "";
            bool exists = false;
            if (string.IsNullOrWhiteSpace(database))
                return exists;
            query = string.Format("SELECT * FROM sys.databases WHERE name='{0}'", database);
            exists = ExecuteQuery(query, new System.Data.DataTable(), true) > 0;
            return exists;
        }

        /// <summary>
        /// Returns a value indicating whether the data base exists.
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExists()
        {
            string query = string.Format("SELECT * FROM sys.databases WHERE name='{0}'", Database);
            DataTable dt = new DataTable();
            if (ExecuteQuery(query, dt, true) > 0)
                return true;
            else
                return false;
        }

    }
}
