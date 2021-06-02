using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data
{
    public class dloUserGroups: ObservableCollection<dloUserGroup>
    {
        #region Members
        private dloDataApplication _app;
        #endregion

        #region Properties
        public bool IsLoaded { get; private set; }
        internal dloDataApplication Application { get { return _app; } }
        #endregion
        internal dloUserGroups(dloDataApplication app)
        {
            _app = app;
        }

        public dloUserGroup Add()
        {
            dloUserGroup u = new dloUserGroup(this);
           
            base.Add(u);
            return u;
        }

        internal void Load()
        {
            string sql = "SELECT * FROM dsto_groups";
            DataTable table = new System.Data.DataTable();
            _app.DbInfo.ExecuteQuery(sql, table);

            foreach (DataRow dr in table.Rows)
            {
                dloUserGroup group = Add();
                switch (_app.Provider)
                {
                    case DataProviders.SQL:
                        group.Id = ((Guid)dr["guid"]).ToString();
                        break;
                    default:
                        group.Id = (string)dr["guid"];
                        break;
                }
                
                group.Name = (string)dr["name"];
                if (dr["description"] != DBNull.Value)
                    group.Description = (string)dr["description"];
                group.EditMode = ObjectStates.None;
            }
            IsLoaded = true;
        }

        public void Refresh()
        {
            Clear();
            Load();
        }
    }
}
