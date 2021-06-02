using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class ModuleProvider : Provider
    {
        public ModuleProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }
        public override bool Save(AiCollectObject obj)
        {
            Module module = obj as Module;
            var exists = RecordExists("dsto_module", module.Key);
            return !exists ? Insert(module) : Edit(module);
        }

        private bool Edit(Module module)
        {
            string query = $"UPDATE dsto_module SET " +
                    $"name='{module.Name}', " +
                    $"yref_configuration = '{module.ConfigurationId}', " +
                    $"deleted = '{module.Deleted}', " +
                    $"WHERE guid = '{module.Key}'";
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        private bool Insert(Module module)
        {
            try
            {
                string query = $"INSERT INTO dsto_module (guid,created_by,name,yref_configuration) values('{module.Key}','Admin', '{module.Name}',{module.ConfigurationId})";
                if (DbInfo.ExecuteNonQuery(query) > 0)
                {
                    module.OID = (int)ExecuteScalar("dsto_module", "oid", module.Key);
                    var qnProvider = new QuestionaireProvider(DbInfo);
                    foreach (var questionaire in module.Questionaires)
                    {
                        questionaire.ModuleId = module.OID;
                        qnProvider.SaveQuestionaire(questionaire);
                    }
                    var dataLinkProvider = new DataLinkProvider(DbInfo);
                    foreach (var dataLink in module.DataLinks)
                    {
                        dataLink.OriginObject = module.Key;
                        dataLinkProvider.Save(dataLink);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetModule(Module module, DataRow row)
        {
            module.Key = row["guid"].ToString();
            module.CreatedBy = row["created_by"].ToString();
            module.OID = int.Parse(row["oid"].ToString());
            module.Name = row["name"].ToString();      
            new QuestionaireProvider(DbInfo).GetQuestionairesByModule(module);
            new DataLinkProvider(DbInfo).GetModuleLinks(module);
        }

        public Module RetrieveModule(int id)
        {
            Module module = new Module();
            try
            {
                string query = string.Format("select * from dsto_module where oid='{0}'", id);
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    SetModule(module, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return module;
        }

        public Modules RetrieveModules(int configuratioId)
        {
            Modules modules = new Modules();
            try
            {
                string query = $"select * from dsto_module where yref_configuration='{configuratioId}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Module module = modules.Add();
                        SetModule(module, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return modules;
        }

    }

}
