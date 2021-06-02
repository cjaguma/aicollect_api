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
    public class PurchaseProvider : Provider
    {
        public PurchaseProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }

        public Purchase GetPurchase(int id)
        {
            string query = $"select * from dsto_purchase where oid='{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];

                Purchase purchase = new Purchase();
                InitPurchases(purchase, row);
                return purchase;
            }
            return null;
        }

        public Reports GetReports(string response_id)
        {
            try
            {
                Reports reports = new Reports();
                string query = $"select dsto_questionaire.guid as questionaire_guid, dsto_questionaire.oid as questionaire_oid, dsto_purchase.* from dsto_questionaire inner join dsto_purchase on dsto_questionaire.guid = dsto_purchase.farmerid where dsto_questionaire.yref_template='{response_id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Report report = reports.Add();
                        report.Questionaire = new Questionaire(null);
                        report.Questionaire.Key = row["questionaire_guid"].ToString();
                        report.Questionaire.OID = int.Parse(row["questionaire_oid"].ToString());
                        report.Questionaire.Name = new SectionProvider(DbInfo).QuestionaireIdentification(response_id, report.Questionaire.Key);

                        report.Purchase = new Purchase();
                        InitPurchases(report.Purchase, row);
                    }
                }
                return reports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPurchases(Purchase purchase, DataRow row)
        {
            purchase.Key = row["guid"].ToString();
            purchase.CreatedBy = row["created_by"].ToString();
            purchase.OID = int.Parse(row["oid"].ToString());
            purchase.Deleted = bool.Parse(row["deleted"].ToString());
            purchase.Price = decimal.Parse(row["price"].ToString());
            purchase.DateOfPurchase = DateTime.Parse(row["dateofpurchase"].ToString());
            purchase.Quantity = int.Parse(row["quantity"].ToString());
            purchase.Lotid = row["lotid"].ToString();
            purchase.Farmer = row["farmerid"].ToString();
            purchase.ConfigurationId = row["configuration_id"].ToString();
            purchase.Product = int.Parse(row["product"].ToString());
            purchase.Station = int.Parse(row["station"].ToString());
        }

        public Purchases GetPurchases(int Id)
        {
            Purchases purchases = new Purchases();
            try
            {
                string query = $"select * from dsto_purchase where configuration_Id='{Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    System.Data.DataRow row = table.Rows[0];
                    Purchase purchase = purchases.Add();
                    InitPurchases(purchase, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return purchases;
        }

        public override bool Save(AiCollectObject obj)
        {
            Purchase purchase = obj as Purchase;

            string query = string.Empty;

            var exists = RecordExists("dsto_purchase", purchase.Key);
            if (!exists)
            {
                query = $"insert into dsto_purchase(guid,created_by,price,dateofpurchase,quantity,farmerid,lotid,configuration_id,product,station) values('{purchase.Key}','{purchase.CreatedBy}','{purchase.Price}','{purchase.DateOfPurchase.ToString("yyyy-MM-dd HH:mm:ss.fff")}','{purchase.Quantity}','{purchase.Farmer}','{purchase.Lotid}','{purchase.ConfigurationId}','{purchase.Product}','{purchase.Station}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_purchase SET price='{purchase.Price}', " +
                        $"dateofpurchase='{purchase.DateOfPurchase.ToString("yyyy-MM-dd HH:mm:ss.fff")}', " +
                        $"quantity='{purchase.Quantity}', " +
                        $"lotid= '{purchase.Lotid}', " +
                        $"farmerid= '{purchase.Farmer}', " +
                        $"product='{purchase.Product}', " +
                        $"station='{purchase.Station}', " +
                        $"deleted='{purchase.Deleted}' " +
                        $"WHERE guid='{purchase.Key}'";
            }

            if (DbInfo.ExecuteNonQuery(query) > -1)
            {

                return true;
            }

            return false;
        }

        public bool DeletePurchase(int id)
        {
            string query = $"delete from dsto_purchase where oid='{id}'";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
