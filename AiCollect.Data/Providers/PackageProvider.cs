using AiCollect.Core;
using AiCollect.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class PackageProvider : Provider
    {
        public PackageProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public override bool Save(AiCollectObject obj)
        {
            Package package = obj as Package;
            var exists = RecordExists("dsto_Package", package.Key);
            return !exists ? Insert(package) : Edit(package);
        }

        private bool Edit(Package package)
        {
            string query = $"UPDATE dsto_package SET name='{package.Name}', " +
                            $"plan='{(int)package.Plan}', " +
                            $"price='{package.Price}', " +
                            $"deleted='{package.Deleted}' " +
                            $"WHERE guid='{package.Key}'";
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        private bool Insert(Package package)
        {
            try
            {
                string query = $"INSERT INTO dsto_package (guid,created_by,name,plan,price) values('{package.Key}','Admin','{package.Name}',{(int)package.Plan},'{package.Price}')";
                return DbInfo.ExecuteNonQuery(query) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPackage(Package package, DataRow row)
        {
            package.Key = row["guid"].ToString();
            package.CreatedBy = row["created_by"].ToString();
            package.OID = int.Parse(row["oid"].ToString());
            package.Name = row["name"].ToString();
            package.Deleted = bool.Parse(row["deleted"].ToString());
            package.Plan = (Plan)Enum.Parse(typeof(Plan), row["plan"].ToString());
            package.Price = decimal.Parse(row["price"].ToString());
        }

        public Packages RetrievePackages()
        {
            Packages packages = new Packages();
            try
            {
                string query = $"select * from dsto_package where deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Package package = packages.Add();
                        InitPackage(package, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return packages;
        }

        public Package RetrievePackage(string key)
        {
            string query = string.Format("select * from dsto_package where guid='{0}'", key);
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                Package package = new Package();
                package.Key = row["guid"].ToString();
                package.CreatedBy = row["created_by"].ToString();
                package.Deleted = bool.Parse(row["deleted"].ToString());
                package.OID = int.Parse(row["oid"].ToString());
                package.Name = row["name"].ToString();
                package.Plan = (Plan)Enum.Parse(typeof(Plan), row["plan"].ToString());
                package.Price = decimal.Parse(row["price"].ToString());

                return package;
            }

            return null;
        }
    }
}
