using AiCollect.Data.Providers;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class SkipConditionProvider : Provider
    {
        public SkipConditionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public SkipConditions GetConditions(Question question)
        {
            SkipConditions conditions = new SkipConditions(null);
            try
            {
                string query = $"select * from dsto_skipcondition where yref_question='{question.Key}' and deleted=false order by oid asc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        SkipCondition condition = new SkipCondition(null);
                        condition.Target = new Target();
                        condition.Key = row["guid"].ToString();
                        condition.OID = int.Parse(row["oid"].ToString());
                        condition.Deleted = bool.Parse(row["deleted"].ToString());
                        condition.Answer = new EnumListValueProvider(DbInfo).GetEnumList(row["answer"].ToString());
                        condition.DataCollectionObectType = (row["dataCollectionObectType"] != DBNull.Value) ? (DataCollectionObectTypes)Enum.Parse(typeof(DataCollectionObectTypes), row["dataCollectionObectType"].ToString()) : DataCollectionObectTypes.None;
                        
                        switch(condition.DataCollectionObectType)
                        {
                            case DataCollectionObectTypes.Section:
                                condition.Target.Section = new SectionProvider(DbInfo).GetTargetSection(row["yref_target"].ToString());
                                break;
                            case DataCollectionObectTypes.SubSection:
                                condition.Target.SubSection = new SubSectionProvider(DbInfo).GetSubSection(row["yref_target"].ToString());
                                break;
                            case DataCollectionObectTypes.Question:
                                condition.Target.Question = new QuestionProvider(DbInfo).GetQuestion(row["yref_target"].ToString());
                                break;
                        }
                        
                        condition.AttributeKey = row["yref_attribute"].ToString();
                        conditions.Add(condition);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return conditions;
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                SkipCondition condition = obj as SkipCondition;
                var exists = RecordExists("dsto_skipcondition", condition.Key);
                string query = string.Empty;
                string targetKey = (condition.Target.Section != null) ? condition.Target.Section.Key : (condition.Target.SubSection != null) ? condition.Target.SubSection.Key : condition.Target.Question.Key;
                if (!exists)
                {
                    query = $"insert into dsto_skipcondition(guid,created_by,yref_attribute,yref_target,answer,yref_question,dataCollectionObectType) " +
                            $"values('{condition.Key}','Admin','{condition.AttributeKey}','{targetKey}','{condition.Answer.Key}','{condition.QuestionKey}', '{(int)condition.DataCollectionObectType}')";
                }
                else
                {
                    query = $"UPDATE dsto_skipcondition SET " +
                        $"yref_attribute = '{condition.AttributeKey}', " +
                        $"yref_target = '{targetKey}', " +
                        $"dataCollectionObectType = '{(int)condition.DataCollectionObectType}', " +
                        $"answer = '{condition.Answer.Key}', " +
                        $"deleted='{condition.Deleted}' " +
                        $"WHERE guid = '{condition.Key}'";
                }

                return DbInfo.ExecuteNonQuery(query) > -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
