using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class SubSectionProvider : Provider
    {
        public SubSectionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public override bool Save(AiCollectObject obj)
        {
            QuestionProvider provider = new QuestionProvider(DbInfo);

            SubSection subsection = obj as SubSection;

            var exists = RecordExists("dsto_subsections", subsection.Key);
            string query = string.Empty;
            if (!exists)
            {
                query = $"insert into dsto_subsections(guid,Name,created_by,yref_section) values('{subsection.Key}','{subsection.Name}','Admin','{subsection.SectionKey}')";
            }
            else
            {
                //update
                query = $"UPDATE dsto_subsections SET Name='{subsection.Name}', " +
                    $"Deleted='{subsection.Deleted}' " +
                    $"WHERE guid = '{subsection.Key}'";
            }
            if (DbInfo.ExecuteNonQuery(query) > -1)
            {
                //save questions
                foreach (var qn in subsection.Questions)
                {
                    qn.SubSectionKey = subsection.Key;
                    provider.Save(qn);
                }
                return true;
            }

            return false;
        }

        internal SubSections GetSubSections(Section section, string response_id)
        {           
            try
            {
                QuestionProvider provider = new QuestionProvider(DbInfo);
                string query = $"select * from dsto_subsections where yref_section='{section.Key}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    DataRow row = table.Rows[0];
                    SubSection subsection = section.SubSections.Add();
                    subsection.Key = row["guid"].ToString();
                    subsection.OID = int.Parse(row["oid"].ToString());
                    subsection.Name = row["Name"].ToString();
                    subsection.Deleted = bool.Parse(row["deleted"].ToString());
                    subsection.CreatedBy = row["created_by"].ToString();
                    subsection.Questions = provider.GetQuestions(subsection, response_id,0);
                }
                return section.SubSections;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(string reference, params string[] oids)
        {
            string filter = string.Empty;
            int i = 0;
            foreach (var s in oids)
            {
                filter += $"'{s}'";
                if (i < oids.Length - 1)
                    filter += ",";
                i++;
            }
            string query = $"delete from dsto_subsections where guid IN ({filter}) AND (yref_questionaire = '{reference}' or yref_section = '{reference}')";
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }

        public SubSection GetSubSection(string key, int occurance = 0, string response_id = null)
        {
            string query = $"select * from dsto_subsections where guid = '{key}' and deleted=false";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];

                SubSection subsection = new SubSection(null);
                subsection.Key = row["guid"].ToString();
                subsection.OID = int.Parse(row["oid"].ToString());
                subsection.Name = row["Name"].ToString();
                subsection.Deleted = bool.Parse(row["deleted"].ToString());
                subsection.CreatedBy = row["created_by"].ToString();
                subsection.Questions = new QuestionProvider(DbInfo).GetQuestions(subsection, response_id, occurance);
                return subsection;
            }
            return null;
        }
        public bool DeleteSubSection(string key)
        {
            string query = $"delete from dsto_subsections where guid = {key}";
            SubSection subSection = GetSubSection(key);
            string[] guids = new string[subSection.Questions.Count];
            //int i = 0;
            foreach (var s in subSection.Questions)
            {
                //guids[i] = s.Key;
                //i++;
                new QuestionProvider(DbInfo).DeleteQuestion(s.Key);
            }
            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }
    }
}
