using AiCollect.Data.Services;
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
    public class BillingProvider : Provider
    {
        public BillingProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public override bool Save(AiCollectObject obj)
        {
            Billing billing = obj as Billing;
            var exists = RecordExists("dsto_billing", billing.Key);
            return !exists ? Insert(billing) : Edit(billing);
        }

        private bool Edit(Billing billing)
        {
            string query = $"UPDATE dsto_billing SET " +
                    $"bill='{billing.Bill}', " +
                    $"billingDate='{billing.BillingDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', " +
                    $"invoiceNo='{billing.InvoiceNo}', " +
                    $"yref_package='{billing.Package.Key}', " +
                    $"paymentstatus='{(int)PaymentStatus.Paid}', " +
                    $"clientid='{billing.Client.OID}' , " +
                    $"deleted='{billing.Deleted}', " +
                    $"WHERE guid='{billing.Key}'";
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        private bool Insert(Billing billing)
        {
            try
            {
                string query = $"INSERT INTO dsto_billing (guid,created_by,bill,billingdate,yref_package,invoiceno,clientid) values('{billing.Key}','Admin','{billing.Package.Price}', '{billing.BillingDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}','{billing.Package.Key}','{billing.InvoiceNo}','{billing.Client.OID}')";
                if(DbInfo.ExecuteNonQuery(query) > 0)
                {
                    //MailService.SendMail(billing.Client.Email, true, "Client Invoice", "Hello client, your account has been invoinced. <br> You're therefore required to make payment to avoid any inconveniences", "#");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetBilling(Billing billing, DataRow row)
        {
            billing.Key = row["guid"].ToString();
            billing.CreatedBy = row["created_by"].ToString();
            billing.OID = int.Parse(row["oid"].ToString());
            billing.Bill = row["bill"].ToString();
            billing.Deleted = bool.Parse(row["deleted"].ToString());
            billing.BillingDate = DateTime.Parse(row["billingdate"].ToString());
            billing.InvoiceNo = row["invoiceno"].ToString(); 
            billing.PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), row["paymentstatus"].ToString());
            billing.Client = new ClientProvider(DbInfo).GetClient(int.Parse(row["clientId"].ToString()));
            billing.Package = new PackageProvider(DbInfo).RetrievePackage(row["yref_package"].ToString());
        }

        public Billings RetrieveClientBills(string id)
        {
            Billings billings = new Billings();
            try
            {
                string query = string.Format("select * from dsto_billing where clientId='{0}' and deleted=false", id);
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Billing billing = billings.Add();
                        SetBilling(billing, row);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return billings;
        }

        public Billings RetrieveBills()
        {
            Billings billings = new Billings();
            try
            {
                string query = "select * from dsto_billing where deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Billing billing = billings.Add();
                        SetBilling(billing, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return billings;
        }
    }
}
