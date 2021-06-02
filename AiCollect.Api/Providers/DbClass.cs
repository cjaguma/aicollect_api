using AiCollect.Data;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace AiCollect.Api.Providers
{
    public class DbClass
    {
        internal string Database
        {
            get; set;
        }

        internal string Server
        {
            get; private set;
        }

        public string ConnectionString
        {
            get
            {
                return $"server={Server};initial catalog={Database}; integrated security=true";
            }
        }
        public string MasterConnectionString
        {
            get
            {
                StringBuilder sb = new StringBuilder(200);
                sb.Append("Server=");
                sb.Append(Server);
                sb.Append(";Database=Master");
                sb.Append(";Trusted_Connection=True;");
                //Pooling
                sb.Append("Pooling=false;");
                return sb.ToString();
            }
        }
        public SqlConnection Connection { get; set; }
        private SqlConnection _masterConnection;
        public SqlConnection MasterConnection
        {
            get
            {
                if (_masterConnection == null)
                {
                    _masterConnection = new SqlConnection(MasterConnectionString);
                }
                return _masterConnection;
            }
        }
        public DbClass()
        {
            Server = "(local)";
            Connection = new SqlConnection(ConnectionString);
        }

        public bool Open()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch { return false; }
        }

        public void Close()
        {
            Connection.Close();
        }

        public int ExecuteNonQuery(string query)
        {
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteNonQuery();
        }
        public object ExecuteScalar(string query)
        {
            SqlCommand command = Connection.CreateCommand();
            command.CommandText = query;
            return command.ExecuteScalar();
        }

        public bool DatabaseExists(string database = null)
        {
            bool exists = false;
            if (string.IsNullOrWhiteSpace(database))
                return exists;

            string query = string.Format("SELECT * FROM sys.databases WHERE name='{0}'", database);
            exists = ExecuteQuery(query, new System.Data.DataTable(), true) > 0;

            return exists;
        }

        public bool CreateDatabase(Configuration configuration)
        {

            //create
            dloDataApplication dloData = new dloDataApplication();
            dloData.Configuration = configuration;
            dloData.DbInfo.ConnectionString = ConnectionString;
            dloData.DbInfo.MasterConnectionString = MasterConnectionString;
            dloData.GenerateDatabase(false, configuration);

            //execute queries
            try
            {
                return dloData.ExecuteQueries();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteQuery(string query, System.Data.DataTable datatable, bool master = false)
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

            cmd = conn.CreateCommand();
            adapter = new SqlDataAdapter();
            cmd.CommandText = query;
            cmd.Connection = conn;
            adapter.SelectCommand = cmd;
            ret = adapter.Fill(datatable);

            return ret;
        }
    }
}