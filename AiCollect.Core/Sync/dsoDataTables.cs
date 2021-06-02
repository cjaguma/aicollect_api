using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AiCollect.Core.Sync
{
    public class dsoDataTables : ObservableCollection<dsoDataTable>
    {

        public dsoDataTable Add()
        {
            dsoDataTable dt = new dsoDataTable();
            base.Add(dt);
            return dt;
        }

        public dsoDataTable ByKey(string key)
        {
            return this.FirstOrDefault(a => a.Key == key);
        }

        public dsoDataTable this[string name]
        {
            get
            {
                var table = this.FirstOrDefault(t => t.TableName == name);
                return table;
            }
        }

    }
}