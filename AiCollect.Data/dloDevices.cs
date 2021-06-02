using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//susing Datalab

namespace AiCollect.Data
{
    public class dloDevices:ObservableCollection<dloDevice>
    {
        #region Members
        private dloDataApplication _app;
        #endregion

        #region Properties
        public bool IsLoaded { get; private set; }
        #endregion

        #region Constructors
        internal dloDevices(dloDataApplication application)
        {
            _app = application;
        }

        #endregion
        public dloDevice this[string IMEI]
        {
            get { return this.FirstOrDefault(d => d.Imei ==IMEI); }
        }


        public dloDevice Add()
        {
            dloDevice u = new dloDevice(_app);
            base.Add(u);
            return u;
        }

        internal void Load()
        {
            try
            {
                string sql = "SELECT * FROM dsto_devices";
                DataTable table = new System.Data.DataTable();
                _app.DbInfo.ExecuteQuery(sql, table);

                foreach (DataRow dr in table.Rows)
                {
                    var device = Add();
                    device.Description = Convert.ToString(dr["Description"]);
                  
                    device.Name = Convert.ToString(dr["Name"]);
                    device.Imei = Convert.ToString(dr["IMEI"]);
                 
                }
                IsLoaded = true;
            }
            catch (Exception)
            {

            }
            finally
            {
                
            }
        }

        public void Refresh()
        {

            base.Clear();
            Load();
        }

    }
}
