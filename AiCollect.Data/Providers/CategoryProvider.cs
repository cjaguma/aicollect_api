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
    public class CategoryProvider : Provider
    {
        public CategoryProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public override bool Save(AiCollectObject obj)
        {
            Category category = obj as Category;
            var exists = RecordExists("dsto_category", category.Key);
            return !exists ? Insert(category) : Edit(category);
        }

        private bool Edit(Category category)
        {
            string query = $"UPDATE dsto_category SET " +
                    $"name='{category.Name}', " +
                    $"Deleted = '{category.Deleted}', " +
                    $"WHERE guid = '{category.Key}'";
            return DbInfo.ExecuteNonQuery(query) > -1;
        }

        private bool Insert(Category category)
        {
            try
            {
                string query = string.Empty;
                query = $"INSERT INTO dsto_category (guid,name) values('{category.Key}','{category.Name}')";

                if (DbInfo.ExecuteNonQuery(query) > 0)
                {
                    if (!string.IsNullOrEmpty(category.QuestionaireId))
                        new QuestionaireXCategoryProvider(DbInfo).Save(category);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        public Categories RetrieveTemplateCategories()
        {
            Categories categories = new Categories();
            try
            {
                
                string query = "select * from dsto_category where guid not in (select yref_category from dsto_questionairexcategory)";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Category category = categories.Add();
                        SetCategory(category, row);
                    }
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categories;
        }

        private void SetCategory(Category category, DataRow row)
        {
            try
            {
                category.Key = row["guid"].ToString();
                category.OID = int.Parse(row["OID"].ToString());
                category.Deleted = bool.Parse(row["deleted"].ToString());
                category.Name = row["name"].ToString();

                category.Questionaires = new QuestionaireProvider(DbInfo).GetCategoryQuestionaires(category.Key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Categories RetrieveCategories()
        {
            Categories categories = new Categories();
            try
            {               
                string query = "select * from dsto_category where deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Category category = categories.Add();
                        SetCategory(category, row);
                    }
                }           
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return categories;
        }

        public Category RetrieveCategory(string category_id)
        {
            try
            {
                string query = $"select * from dsto_category where guid='{ category_id }'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Category category = new Categories().Add();
                        SetCategory(category, row);
                        return category;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}
