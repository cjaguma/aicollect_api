using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCollect.Core;
using System.Data;

namespace AiCollect.Data.Providers
{
    public class QuestionaireProvider : Provider
    {

        public QuestionaireProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public Questionaire GetQuestionaire(string guid)
        {
            Questionaire questionaire = null;
            try
            {
               
                string query = $"select * from dsto_questionaire where guid='{guid}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0 && table.Rows.Count == 1)
                {
                    DataRow row = table.Rows[0];
                    questionaire = new Questionaire(null);
                    InitQuestionaire(questionaire, row);
                    var sectionProvider = new SectionProvider(DbInfo);
                    questionaire.Sections = sectionProvider.GetSections(questionaire.Key);
                }
               
            }
            catch
            {
               
            }
            return questionaire;
        }

        public Questionaire GetQuestionaire(int id)
        {
            
            string query = $"select * from dsto_questionaire where oid='{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            Questionaire questionaire = null;
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                questionaire = new Questionaire(null);
                InitQuestionaire(questionaire, row);             
            }
            
            return questionaire;
        }

        private void InitQuestionaire(Questionaire questionaire, DataRow row)
        {
            try
            {
                questionaire.Key = row["guid"].ToString();
                questionaire.OID = int.Parse(row["oid"].ToString());
                questionaire.Name = row["Name"].ToString();
                questionaire.Status = (Statuses)Enum.Parse(typeof(Statuses), row["Status"].ToString());
                questionaire.Deleted = bool.Parse(row["deleted"].ToString());
                questionaire.Latitude = Convert.ToDouble(row["Latitude"].ToString());
                questionaire.Longitude = Convert.ToDouble(row["Longitude"].ToString());
                questionaire.CreatedOn = (row["created_on"] != DBNull.Value )? Convert.ToDateTime(row["created_on"].ToString()) : DateTime.MinValue;
                questionaire.Regions = new Regions();

                if (row["yref_template"] != null && row["yref_template"] != DBNull.Value)
                {
                    questionaire.Template = Convert.ToString(row["yref_template"]);
                    questionaire.Sections = new SectionProvider(DbInfo).GetSections(questionaire.Template, questionaire.Key);
                }
                else
                {
                    questionaire.Sections = new SectionProvider(DbInfo).GetSections(questionaire.Key);
                    questionaire.Regions = new RegionProvider(DbInfo).GetRegions(questionaire.Key);
                }

                if (row["yref_category"] != null && row["yref_category"] != DBNull.Value)
                    questionaire.Category = new CategoryProvider(DbInfo).RetrieveCategory(Convert.ToString(row["yref_category"]));

                if (row["yref_region"] != null && row["yref_region"] != DBNull.Value)
                    questionaire.Region = new RegionProvider(DbInfo).RetrieveRegion(Convert.ToString(row["yref_region"]));

                if (questionaire.Region != null)
                    if (row["code"] != null && row["code"] != DBNull.Value && questionaire.Region.OID != 0)
                        questionaire.QuestionaireCode = questionaire.Region.Prefix + "-" + row["code"].ToString();

                questionaire.CreatedBy = row["created_by"].ToString();
                new QuestionaireXCategoryProvider(DbInfo).GetQuestionaireCategories(questionaire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal long LatestCode(string yref_template)
        {
            try
            {
                string query = $"select max(code) as code from dsto_questionaire where yref_template='{yref_template}'";
                var cols = DbInfo.ExecuteSelectQuery(query);
                if (cols.Rows.Count == 1)
                    return Convert.ToInt64(cols.Rows[0]["code"].ToString());
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        public Questionaires GetQuestionaires(int id, bool withTransaction=false)
        {
            Questionaires questionaires = new Questionaires();
            try
            {
                string query = $"select * from dsto_questionaire where status=0 and configuration_id='{id}' and deleted=false";
                if (withTransaction)
                    DbInfo.BeginTransaction();
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        InitQuestionaire(questionaire, row);
                        questionaires.Add(questionaire);
                    }
                }
                if (withTransaction)
                    DbInfo.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (withTransaction)
                    DbInfo.RollBackTransaction();
                throw ex;
            }

            return questionaires;
        }

        public Questionaires ConfigurationQuestionaires(int id)
        {
            Questionaires questionaires = new Questionaires();
            try
            {
                string query = $"select * from dsto_questionaire where status=0 and configuration_id='{id}' and yref_template is null and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        questionaire.Key = row["guid"].ToString();
                        questionaire.OID = int.Parse(row["oid"].ToString());
                        questionaire.Name = row["Name"].ToString();
                        questionaire.CreatedBy = row["created_by"].ToString();
                        questionaire.Status = (Statuses)Enum.Parse(typeof(Statuses), row["Status"].ToString());
                        questionaire.Deleted = bool.Parse(row["deleted"].ToString());
                        questionaire.Latitude = Convert.ToDouble(row["Latitude"].ToString());
                        questionaire.Longitude = Convert.ToDouble(row["Longitude"].ToString());
                        if (row["yref_category"] != null && row["yref_category"] != DBNull.Value)
                            questionaire.Category = new CategoryProvider(DbInfo).RetrieveCategory(Convert.ToString(row["yref_template"]));
                        questionaires.Add(questionaire);
                    }
                }

                return questionaires;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Questionaires GetQuestionairesByConfig(int id)
        {
            Questionaires questionaires = new Questionaires();
            try
            {
                string query = $"select * from dsto_questionaire where configuration_id='{id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        InitQuestionaire(questionaire, row);
                        questionaires.Add(questionaire);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return questionaires;
        }

        public void GetQuestionairesByModule(Module module)
        {
           
            try
            {
                string query = $"select * from dsto_questionaire where yref_module='{module.OID}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        InitQuestionaire(questionaire, row);
                        module.Questionaires.Add(questionaire);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        internal bool Sync(Questionaire questionaire)
        {
            try
            {
                AnswerProvider answerProvider = new AnswerProvider(DbInfo);
                bool saved = SaveQuestionaire(questionaire);
                if (saved)
                    foreach (Section section in questionaire.Sections)
                    {
                        foreach (Question question in section.Questions)
                        {
                            answerProvider.Sync(questionaire, question);
                            var dependencies = question.Dependencies.Where(d => !string.IsNullOrEmpty(d.Template)).ToList();
                            foreach (Dependency dependency in dependencies)
                                foreach (Question qn in dependency.Target.SubSection.Questions)
                                    answerProvider.Sync(questionaire, qn);
                        }

                        foreach (SubSection subSection in section.SubSections)
                            foreach (Question question in subSection.Questions)
                                answerProvider.Sync(questionaire, question);
                    }

                return saved;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Questionaires GetReviewQuestionaires(string id)
        {
            Questionaires questionaires = new Questionaires();
            try
            {
                string query = $"select *, LPAD(code::text, 6, '0') as qcode from dsto_questionaire where (status=2 or status=3) and yref_template='{id}' and deleted=false order by created_on desc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        InitQuestionaire(questionaire, row);
                        questionaire.CreatedBy = new UserProvider(DbInfo).GetCreatedByUser(row["created_by"].ToString());
                        questionaires.Add(questionaire);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return questionaires;
        }

        public Questionaires GetCategoryQuestionaires(string category_id)
        {
            Questionaires questionaires = new Questionaires();
            try
            {
                string query = $"select * from dsto_questionaire where yref_category='{category_id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Questionaire questionaire = new Questionaire(null);
                        InitQuestionaire(questionaire,row);
                        questionaires.Add(questionaire);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return questionaires;
        }

        public bool SaveQuestionaire(Questionaire questionaire)
        {
            try
            {
                var exists = RecordExists("dsto_questionaire", questionaire.Key);

                string query = string.Empty;
                if (!exists)
                {
                    object val = null;
                    object lastCode = null;
                    if (questionaire.Category.OID == 0)
                    {
                        lastCode = val;
                    }
                    else
                    {
                        val = questionaire.Category.Key;
                        lastCode = LatestCode(questionaire.Template) + 1;
                    }
                   
                   
                    if (string.IsNullOrEmpty(questionaire.ConfigurationId))
                        query = $"insert into dsto_questionaire(guid,Name,created_by,latitude,longitude,status,yref_category,yref_region,yref_template,code) values('{questionaire.Key}','{questionaire.Name}','{questionaire.CreatedBy}','{questionaire.Latitude}','{questionaire.Longitude}','{(int)questionaire.Status}'," +
                                $"'{val}','{val}','{questionaire.Template}', '{lastCode}')";
                    else if (!string.IsNullOrEmpty(questionaire.Template))
                        query = $"insert into dsto_questionaire(guid,Name,created_by,configuration_id,yref_module,latitude,longitude,status,yref_template) values('{questionaire.Key}','{questionaire.Name}','{questionaire.CreatedBy}','{questionaire.ConfigurationId}',{questionaire.ModuleId},'{questionaire.Latitude}','{questionaire.Longitude}','{(int)questionaire.Status}','{questionaire.Template}')";
                    else
                        query = $"insert into dsto_questionaire(guid,Name,created_by,configuration_id,yref_module,latitude,longitude,status) values('{questionaire.Key}','{questionaire.Name}','{questionaire.CreatedBy}','{questionaire.ConfigurationId}',{questionaire.ModuleId},'{questionaire.Latitude}','{questionaire.Longitude}','{(int)questionaire.Status}')";
                }
                else
                {
                    if (!string.IsNullOrEmpty(questionaire.Template))
                        query = $"UPDATE dsto_questionaire SET " +
                            $"Name='{questionaire.Name}', " +
                            $"latitude='{questionaire.Latitude}', " +
                            $"longitude='{questionaire.Longitude}', " +
                            $"status='{(int)questionaire.Status}', " +
                            $"yref_template='{questionaire.Template}', " +
                            $"deleted='{questionaire.Deleted}' " +
                            $"WHERE guid='{questionaire.Key}'";
                    else
                        query = $"UPDATE dsto_questionaire SET " +
                           $"Name='{questionaire.Name}', " +
                           $"latitude='{questionaire.Latitude}', " +
                           $"longitude='{questionaire.Longitude}', " +
                           $"status='{(int)questionaire.Status}', " +
                           $"deleted='{questionaire.Deleted}' " +
                           $"WHERE guid='{questionaire.Key}'";
                }

                return DbInfo.ExecuteNonQuery(query) > -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save(AiCollectObject obj)
        {
            try
            {
                SectionProvider sectionProvider = new SectionProvider(DbInfo);
                Questionaire questionaire = obj as Questionaire;

                var saved = SaveQuestionaire(questionaire);
                if (saved)
                {
                    if (questionaire.Status.Equals(Statuses.Template))
                        foreach (var region in questionaire.Regions)
                        {
                            region.yref_questionaire = questionaire.Key;
                            new RegionProvider(DbInfo).Save(region);
                        }

                    foreach (Category category in questionaire.Categories)
                    {
                        category.QuestionaireId = questionaire.Key;
                        new CategoryProvider(DbInfo).Save(category);
                    }

                    foreach (var section in questionaire.Sections)
                    {
                        section.QuestionaireKey = questionaire.Key;
                        sectionProvider.Save(section);
                    }

                    return true;

                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteQuestionaire(string guid)
        {
            Questionaire questionaire = GetQuestionaire(guid);
            string query = $"delete from dsto_questionaire where guid='{guid}'";
            foreach (var s in questionaire.Sections)
            {
                new SectionProvider(DbInfo).DeleteSection(s.Key);
            }
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }
    }
}
