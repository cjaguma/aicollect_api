using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using AiCollect.Core;
using AiCollect.Data.Providers;

namespace AiCollect.Data.Providers
{
    public class ClientProvider : Provider
    {
        private dloDbInfo dbInfo;
        public ClientProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
            this.dbInfo = dbInfo;
        }

        public override bool Save(AiCollectObject obj)
        {
            return Insert(obj as Client);
        }

        private bool Insert(Client client)
        {
            try
            {
                client.Logo = string.IsNullOrEmpty(client.Logo) ? "" : client.Logo;

                var cmd = DbInfo.CreateDbCommand();
                cmd.CommandText = string.Format("select oid from dsto_client where guid='{0}'", client.Key);
                var oid = DbInfo.ExecuteScalar(cmd);

                string query = string.Empty;
                var exists = oid != null;
                if (!exists)
                    query = $"INSERT INTO dsto_client (guid,created_by,Name,Email,Logo,Contact,Location,yref_package) values('{client.Key}','Admin','{client.Name}','{client.Email}','{client.Logo}','{client.Contact}','{client.Location}','{client.Package?.Key}')";
                else
                    query = $"UPDATE dsto_client SET Name='{client.Name}', " +
                        $"Email='{client.Email}', " +
                        $"Contact='{client.Contact}', " +
                        $"Location='{client.Location}', " +
                        $"Logo='{client.Logo}', " +
                        $"Deleted='{client.Deleted}', " +
                        $"yref_package='{client.Package.Key}' " +
                        $"WHERE oid='{client.OID}'";

                var added = DbInfo.ExecuteNonQuery(query) > 0;
                if (added)
                {
                    oid = DbInfo.ExecuteScalar(cmd);
                    client.OID = (int)oid;

                    if (!exists)
                    {
                        Billing billing = new Billing();
                        billing.Package = client.Package;
                        billing.PaymentStatus = PaymentStatus.Pending;
                        billing.Client = client;
                        billing.Bill = client.Package.Price.ToString();
                        billing.BillingDate = DateTime.Now;
                        billing.InvoiceNo = "XYZ";

                        new BillingProvider(DbInfo).Save(billing);
                    }

                    foreach (var user in client.Users)
                    {
                        user.ClientId = client.OID.ToString();
                        new UserProvider(DbInfo).AddOrUpdateUser(user);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CreateDatabase(Client client)
        {
            DatabaseCreator creator = new DatabaseCreator(this.dbInfo, client);
            creator.CreateDatabase();
        }

        public Client GetClient(int id)
        {
            
            Client client = null;
            try
            {
                string query = string.Format("select * from dsto_client where oid='{0}'", id);
                var table = DbInfo.ExecuteSelectQuery(query);

                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    client = new Client();
                    client.Key = row["guid"].ToString();
                    client.OID = int.Parse(row["oid"].ToString());
                    client.Name = row["name"].ToString();
                    client.Contact = row["contact"].ToString();
                    client.Deleted = bool.Parse(row["deleted"].ToString());
                    client.Location = row["Location"].ToString();
                    //Set Logo
                    if (row["Logo"] != DBNull.Value)
                        client.Logo = row["Logo"].ToString();
                    client.Email = row["Email"].ToString();
                    client.CreatedBy = row["created_by"].ToString();
                    client.Users = new Users(client.Parent);
                    client.Package = new PackageProvider(DbInfo).RetrievePackage(row["yref_package"].ToString());

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return client;
        }

        public Clients GetClients()
        {
            Clients clients = new Clients();
            try
            {
                
                string query = $"select * from dsto_client where deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Client client = new Client();
                        client.Key = row["guid"].ToString();
                        client.OID = int.Parse(row["oid"].ToString());
                        client.Name = row["name"].ToString();
                        client.Contact = row["contact"].ToString();
                        client.Location = row["Location"].ToString();
                        client.Deleted = bool.Parse(row["deleted"].ToString());

                        //Set Logo
                        if (row["Logo"] != DBNull.Value)
                            client.Logo = row["Logo"].ToString();

                        client.Email = row["Email"].ToString();
                        client.CreatedBy = row["created_by"].ToString();
                        client.Users = new UserProvider(DbInfo).ClientAdmins(client.OID);
                        client.Package = new PackageProvider(DbInfo).RetrievePackage(row["yref_package"].ToString());
                        clients.Add(client);
                    }
                }
                
            }
            catch (Exception ex)
            {
                
            }
            return clients;
        }

        public bool Delete(int id)
        {
            string query = $"delete from dsto_client where oid = {id}";
            Client client = GetClient(id);
            foreach (var s in client.Users)
            {
                new UserProvider(DbInfo).DeleteUser(s.OID);
            }
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }

    }
}
