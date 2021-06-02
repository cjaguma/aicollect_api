using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class RegionProvider : Provider
    {
        private dloDbInfo dbInfo;
        public RegionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
            this.dbInfo = dbInfo;
        }

        public override bool Save(AiCollectObject obj)
        {
            return Insert(obj as Region);
        }

        private bool Insert(Region region)
        {
            try
            {
                var cmd = DbInfo.CreateDbCommand();
                cmd.CommandText = string.Format("select oid from dsto_region where guid='{0}'", region.Key);
                var oid = DbInfo.ExecuteScalar(cmd);

                string query = string.Empty;
                var exists = oid != null;
                if (!exists)
                    query = $"INSERT INTO dsto_region (guid,created_by,Name,Prefix, yref_questionaire) values('{region.Key}','Admin','{region.Name}','{region.Prefix}', '{region.yref_questionaire}')";
                else
                    query = $"UPDATE dsto_region SET Name='{region.Name}', " +
                        $"Prefix='{region.Prefix}', " +
                        $"Deleted='{region.Deleted}' " +
                        $"WHERE oid='{region.OID}'";

                var added = DbInfo.ExecuteNonQuery(query) > 0;
                if (added)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Regions GetRegions(string questionaire_id)
        {
            Regions regions = new Regions();
            try
            {
                string query = $"select * from dsto_region where deleted=false and yref_questionaire='{ questionaire_id }'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Region region = regions.Add();
                        InitRegion(region, row);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return regions;
        }

        internal Region RetrieveRegion(string region_id)
        {
            try
            {
                string query = $"select * from dsto_region where guid='{ region_id }'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Region region = new Regions().Add();
                        InitRegion(region, row);

                        return region;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        internal void InitRegion(Region region, DataRow row)
        {
            try
            {
                region.Key = row["guid"].ToString();
                region.OID = int.Parse(row["oid"].ToString());
                region.Name = row["name"].ToString();
                region.Prefix = row["prefix"].ToString();
                region.Deleted = bool.Parse(row["deleted"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
