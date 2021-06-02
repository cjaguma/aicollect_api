using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class TemplateProvider : Provider
    {
        public TemplateProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public override bool Save(AiCollectObject obj)
        {
            Template template = obj as Template;
            var exists = RecordExists("dsto_template", template.Key);
            return !exists ? Insert(template) : Edit(template);
        }

        private bool Edit(Template template)
        {
            try
            {
                string query = $"UPDATE dsto_template SET " +
                        $"description='{template.Description}', " +
                        $"name = '{template.Name}', " +
                        $"templateType = {(int)template.TemplateType}, " +
                        $"yref_category = '{template.Category.Key}', " +
                        $"deleted='{template.Deleted}' " +
                        $"WHERE guid = '{template.Key}'";

                if (DbInfo.ExecuteNonQuery(query) > 0)
                {
                    foreach (var section in template.Sections)
                    {
                        section.TemplateKey = template.Key;
                        new SectionProvider(DbInfo).Save(section);
                    }
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(Template template)
        {
            try
            {
                string query = $"INSERT INTO dsto_template (guid,created_by,description,name,templateType,yref_category) values('{template.Key}','Admin','{template.Description}', '{template.Name}','{(int)template.TemplateType}','{template.Category.Key}')";
                if (DbInfo.ExecuteNonQuery(query) > 0)
                {
                    foreach (var section in template.Sections)
                    {
                        section.TemplateKey = template.Key;
                        new SectionProvider(DbInfo).Save(section);
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

        private void SetTemplate(Template template, DataRow row)
        {
            template.Key = row["guid"].ToString();
            template.CreatedBy = row["created_by"].ToString();
            template.OID = int.Parse(row["oid"].ToString());
            template.Description = row["description"].ToString();
            template.Name = row["name"].ToString();
            template.Deleted = bool.Parse(row["deleted"].ToString());
            template.TemplateType = (TemplateTypes)Enum.Parse(typeof(TemplateTypes), row["TemplateType"].ToString());
            template.Category = new CategoryProvider(DbInfo).RetrieveCategory(row["yref_category"].ToString());
            template.Sections = new SectionProvider(DbInfo).GetSections(template.Key);
        }

        public Templates RetrieveTemplates()
        {
            Templates templates = new Templates();
            try
            {
                string query = "select * from dsto_template where deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Template category = templates.Add();
                        SetTemplate(category, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return templates;
        }

        public Template RetrieveTemplate(string id)
        {
            Template template = null;
            try
            {
                string query = $"select * from dsto_template where oid='{id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    template = new Template();
                    SetTemplate(template, row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return template;
        }

    }
}
