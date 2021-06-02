using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class DependencyProvider : Provider
    {
        public DependencyProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public Dependency GetDependency(string id)
        {
            string query = $"select * from dsto_dependency where guid='{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                Dependency dependency = new Dependency(null);
                InitDependency(dependency, row);
                return dependency;
            }
            return null;
        }

        private void InitDependency(Dependency dependency, DataRow row, int occurance = 0, string response_id = null)
        {
            try
            {
                dependency.Key = row["guid"].ToString();
                dependency.CreatedBy = row["created_by"].ToString();
                dependency.OID = int.Parse(row["oid"].ToString());
                if (row["targetobjecttype"] != DBNull.Value)
                    dependency.TargetObjectType = (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), row["targetobjecttype"].ToString());
                if (row["targetobjectkey"] != DBNull.Value)
                    dependency.TargetObjectKey = Convert.ToString(row["targetobjectkey"]);
                if (row["yref_question"] != DBNull.Value)
                    dependency.QuestionKey = Convert.ToString(row["yref_question"]);

                dependency.Target = new Target();
                switch (dependency.TargetObjectType)
                {
                    case DataCollectionObectTypes.SubSection:
                        dependency.Target.SubSection = new SubSectionProvider(DbInfo).GetSubSection(dependency.TargetObjectKey, occurance, response_id);
                        break;
                    case DataCollectionObectTypes.Section:
                        dependency.Target.Section = new SectionProvider(DbInfo).GetSection(dependency.TargetObjectKey);
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Dependencies GetDependencyByQuestionId(Question question, Answers answers = null, string response_id = null)
        {
            Dependencies dependencies = new Dependencies(null);
            try
            {
                if (answers.Count > 0)
                {
                    if(RecordExists("dsto_dependency", "yref_question", question.Key))
                        if (!string.IsNullOrEmpty(answers.FirstOrDefault().AnswerText) && int.TryParse(answers.FirstOrDefault().AnswerText, out int n) && question.DataType.Equals(DataTypes.Numeric))
                            for (int occurance = 0; occurance < int.Parse(answers.FirstOrDefault().AnswerText); occurance++)
                            {
                                string query = $"select * from dsto_dependency where yref_question='{question.Key}' and deleted=false";
                                var table = DbInfo.ExecuteSelectQuery(query);
                                if (table.Rows.Count > 0)
                                {
                                    foreach (DataRow row in table.Rows)
                                    {
                                        Dependency dependency = dependencies.Add();
                                        InitDependency(dependency, row, occurance, response_id);
                                    }
                                }
                            }
                }
                else
                {
                    string query = $"select * from dsto_dependency where yref_question='{question.Key}' and deleted=false";
                    var table = DbInfo.ExecuteSelectQuery(query);
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            Dependency dependency = dependencies.Add();
                            InitDependency(dependency, row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dependencies;
        }

        public override bool Save(AiCollectObject obj)
        {
            Dependency dependency = obj as Dependency;
            var exists = RecordExists("dsto_dependency", dependency.Key);
            string query = string.Empty;
            if (!exists)
            {
                query = $"insert into dsto_dependency(guid,created_by,targetobjecttype,yref_question,targetobjectkey) values('{dependency.Key}','Admin',{(int)dependency.TargetObjectType},'{dependency.QuestionKey}','{dependency.TargetObjectKey}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_dependency SET targetobjecttype={(int)dependency.TargetObjectType}, " +
                        $"targetobjectkey='{dependency.TargetObjectKey}' " +
                        $"WHERE guid='{dependency.Key}'";
            }

            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        public bool DeleteDependency(string key)
        {
            string query = $"delete from dsto_dependency where guid='{key}'";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }


    }
}
