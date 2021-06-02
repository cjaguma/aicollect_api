using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class QuestionaireXCategoryProvider : Provider
    {
        public QuestionaireXCategoryProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }

        internal bool Save(Category category)
        {
            try
            {
                var cmd = DbInfo.CreateDbCommand();
                var missing = Exists(category) == false;
                if (missing)
                {
                    string query = $"INSERT INTO dsto_questionaireXcategory (guid,created_by,yref_questionaire,yref_category) values('{Guid.NewGuid().ToString()}','Admin','{category.QuestionaireId}','{category.Key}')";
                    return DbInfo.ExecuteNonQuery(query) > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Exists(Category category)
        {
            var query = $"SELECT * FROM dsto_questionaireXcategory WHERE yref_questionaire='{category.QuestionaireId}' and yref_category='{category.Key}' ";
            return DbInfo.ExecuteSelectQuery(query).Rows.Count > 0;
        }

        public Questionaire GetQuestionaireCategories(Questionaire questionaire,bool withTransaction = false)
        {
            try
            {
                string query = $"select * from dsto_questionaireXcategory where yref_questionaire='{questionaire.Key}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Category category = new CategoryProvider(DbInfo).RetrieveCategory(Convert.ToString(row["yref_category"]));
                        questionaire.Categories.Add(category);
                    }
                    return questionaire;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
