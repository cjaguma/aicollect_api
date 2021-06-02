using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using AiCollect.Core;

namespace AiCollect.Data.Providers
{
    public abstract class Provider
    {
        private dloDbInfo _dbInfo;

        internal dloDbInfo DbInfo
        {
            get
            {
                return _dbInfo;
            }
            private set
            {
                _dbInfo = value;
            }
        }

        public Provider(dloDbInfo dbInfo)
        {
            _dbInfo = dbInfo;
        }

        public virtual bool Save(AiCollectObject obj)
        {
            return true;
        }

        public object ExecuteScalar(string table, string column, string key)
        {
            SqlCommand cmd = _dbInfo.CreateSqlCommand() as SqlCommand;
            cmd.CommandText = $"SELECT {column} FROM {table} WHERE guid='{key}'";
            return _dbInfo.ExecuteScalar(cmd);
        }

        public bool RecordExists(string table, string key, bool withTransaction=false)
        {
            var query = $"SELECT * FROM {table} WHERE guid='{key}'";
            return _dbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

        public bool RecordExists(string table, string column, string key,bool withTransaction=false)
        {
            var query = $"SELECT * FROM {table} WHERE {column}='{key}'";
            return _dbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

        public bool RecordExists(string table, int oid)
        {
            var query = $"SELECT * FROM {table} WHERE oid={oid}";
            return _dbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

        public virtual bool SoftDelete(string table, string key)
        {
            var query = $"UPDATE {table} SET deleted = 1 WHERE guid='{key}'";
            return _dbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

        public virtual bool SoftDelete(string table, int oid)
        {
            var query = $"UPDATE {table} SET deleted = 1 WHERE oid={oid}";
            return _dbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

    }
}
