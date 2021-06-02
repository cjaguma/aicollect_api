using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class SectionProvider : Provider
    {
        public SectionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }

        public Section GetSection(string id, string response_id = null)
        {
            string query = $"select * from dsto_sections where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                Section section = new Section(null);
                section.Key = row["guid"].ToString();
                section.OID = int.Parse(row["oid"].ToString());
                section.Name = row["Name"].ToString();
                section.Deleted = bool.Parse(row["deleted"].ToString());
                section.CreatedBy = row["created_by"].ToString();
                section.Questions = new QuestionProvider(DbInfo).GetQuestions(section, response_id);
                section.SubSections = new SubSectionProvider(DbInfo).GetSubSections(section, response_id);
                return section;
            }
            return null;
        }

        public Section GetTargetSection(string key)
        {
            string query = $"select * from dsto_sections where guid = '{key}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                Section section = new Section(null);
                section.Key = row["guid"].ToString();
                section.OID = int.Parse(row["oid"].ToString());
                section.Name = row["Name"].ToString();
                section.Deleted = bool.Parse(row["deleted"].ToString());
                section.CreatedBy = row["created_by"].ToString();
                return section;
            }
            return null;
        }

        public override bool Save(AiCollectObject obj)
        {
            QuestionProvider qnProvider = new QuestionProvider(DbInfo);
            SubSectionProvider sbProvider = new SubSectionProvider(DbInfo);

            Section section = obj as Section;

            var exists = RecordExists("dsto_sections", section.Key);

            string query = string.Empty;

            if (!exists)
            {
                if (!string.IsNullOrEmpty(section.QuestionaireKey))
                    query = $"insert into dsto_sections(guid,Name,Description,created_by,isCompleted,yref_questionaire) values('{section.Key}','{section.Name}','{section.Description}','Admin',{section.IsCompleted},'{section.QuestionaireKey}')";
                else if (!string.IsNullOrEmpty(section.CertificationKey))
                    query = $"insert into dsto_sections(guid,Name,Description,created_by,isCompleted,yref_certification) values('{section.Key}','{section.Name}','{section.Description}','Admin',{section.IsCompleted},'{section.CertificationKey}')";
                else if (!string.IsNullOrEmpty(section.InspectionKey))
                    query = $"insert into dsto_sections(guid,Name,Description,created_by,isCompleted,yref_field_inspection) values('{section.Key}','{section.Name}','{section.Description}','Admin',{section.IsCompleted},'{section.InspectionKey}')";
                else if (!string.IsNullOrEmpty(section.TemplateKey))
                    query = $"insert into dsto_sections(guid,Name,Description,created_by,isCompleted,yref_template) values('{section.Key}','{section.Name}','{section.Description}','Admin',{section.IsCompleted},'{section.TemplateKey}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_sections SET Name='{section.Name}', " +
                    $"Description='{section.Description}', " +
                    $"iscompleted='{section.IsCompleted}', " +
                    $"Deleted='{section.Deleted}' " +
                    $"WHERE guid = '{section.Key}'";
            }

            if (DbInfo.ExecuteNonQuery(query) > -1)
            {
                //save subsections
                foreach (var sb in section.SubSections)
                {
                    sb.SectionKey = section.Key;
                    sbProvider.Save(sb);
                }
                //save questions
                foreach (var qn in section.Questions)
                {
                    qn.SectionKey = section.Key;
                    qnProvider.Save(qn);
                }
                return true;
            }

            return false;
        }

        internal string QuestionaireIdentification(string reference, string response_id)
        {
            try
            {
                string query = $"select top 1 * from dsto_sections where yref_questionaire='{reference}' order by oid asc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                     return new QuestionProvider(DbInfo).QuestionaireIndentification(table.Rows[0]["guid"].ToString(), response_id);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(string reference, params string[] guids)
        {
            string filter = string.Empty;
            int i = 0;
            foreach (var s in guids)
            {
                filter += $"'{s}'";
                if (i < guids.Length - 1)
                    filter += ",";
                i++;
            }
            string query = $"delete from dsto_sections where guid IN ({filter}) AND (yref_questionaire = '{reference}' or yref_field_inspection = '{reference}' or yref_certification = '{reference}')";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }

        public bool DeleteSection(string key)
        {
            string query = $"delete from dsto_sections where guid='{key}'";
            Section section = GetSection(key);
            
            foreach (var s in section.Questions)
            {
                new QuestionProvider(DbInfo).DeleteQuestion(s.Key);
            }

            foreach (var s in section.SubSections)
            {
                new SubSectionProvider(DbInfo).DeleteSubSection(section.Key);
            }

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

        public bool DeleteByReference(string reference)
        {
            string query = $"delete from dsto_sections where yref_questionaire = '{reference}' or yref_field_inspection = '{reference}' or yref_certification = '{reference}'";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }


        internal Sections GetSections(string reference, string response_id = null)
        {
            try
            {
                QuestionProvider provider = new QuestionProvider(DbInfo);
                Sections sections = new Sections();
                string query = $"select * from dsto_sections where (yref_questionaire='{reference}' or yref_field_inspection='{reference}' or yref_certification='{reference}' or yref_template='{reference}') and deleted=false order by oid asc";

                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Section section = new Section(null);
                        section.Key = row["guid"].ToString();
                        section.OID = int.Parse(row["oid"].ToString());
                        section.Name = row["Name"].ToString();
                        section.Description = row["Description"].ToString();
                        section.Deleted = bool.Parse(row["deleted"].ToString());
                        section.CreatedBy = row["created_by"].ToString();
                        section.Questions = provider.GetQuestions(section, response_id);
                        section.SubSections = new SubSectionProvider(DbInfo).GetSubSections(section, response_id);
                        sections.Add(section);
                    }
                    return sections;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
