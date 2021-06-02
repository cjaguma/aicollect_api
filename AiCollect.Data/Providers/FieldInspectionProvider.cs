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
    public class FieldInspectionProvider : Provider
    {
        public FieldInspectionProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }
        public FieldInspection GetInspection(int id, string template_id = null)
        {
          
            string query = $"select * from dsto_fieldInspection where oid='{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                System.Data.DataRow row = table.Rows[0];
                FieldInspection inspection = new FieldInspection();
                InitFieldInspection(inspection, row);
                inspection.Sections = new SectionProvider(DbInfo).GetSections(inspection.Key, template_id);
                return inspection;
            }
           
            return null;
        }

        public FieldInspections GetFieldInspections(int configuration_Id)
        {

            FieldInspections inspections = new FieldInspections();
            try
            {
               
                string query = $"select * from dsto_fieldInspection where status=0 and configuration_id='{configuration_Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        FieldInspection inspection = inspections.Add();
                        InitFieldInspection(inspection, row);
                        inspection.Sections = new SectionProvider(DbInfo).GetSections(inspection.Key, null);
                    }
                }
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return inspections;
        }

        public FieldInspections ConfigurationFieldInspections(int configuration_Id)
        {
            FieldInspections inspections = new FieldInspections();
            try
            {
                string query = $"select * from dsto_fieldInspection where status=0 and configuration_id='{configuration_Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        FieldInspection inspection = inspections.Add();
                        InitFieldInspection(inspection, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inspections;
        }

        public Reports GetReports(string response_id)
        {
            try
            {
               
                Reports reports = new Reports();
                string query = $"select dsto_questionaire.Longitude as questionaire_lng, dsto_questionaire.Latitude as questionaire_lat, " +
                    $"dsto_questionaire.deleted as questionaire_deleted, dsto_questionaire.Status as quesitonaire_status, " +
                    $"dsto_questionaire.Name as questionaire_name, dsto_questionaire.oid as questionaire_oid, " +
                    $"dsto_questionaire.guid as questionaire_guid, dsto_fieldInspection.guid as inspection_key, " +
                    $"dsto_fieldInspection.created_by as inspection_owner, dsto_fieldInspection.oid as inspection_oid, dsto_fieldInspection.created_on as submitted_on, " +
                    $"dsto_fieldInspection.deleted as inspection_deleted, dsto_fieldInspection.status as inspection_status, " +
                    $"dsto_fieldInspection.fieldname as fieldname, dsto_fieldInspection.yref_template as inspection_template " +
                    $"from dsto_fieldInspection inner join dsto_Questionaire on dsto_fieldInspection.farmerid = dsto_Questionaire.guid " +
                    $"where dsto_questionaire.yref_template='{response_id}' order by dsto_fieldInspection.created_on desc";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        Report report = reports.Add();
                        report.Questionaire = new Questionaire(null);
                        report.Questionaire.Key = row["questionaire_guid"].ToString();
                        report.Questionaire.OID = int.Parse(row["questionaire_oid"].ToString());
                        report.Questionaire.Name = new SectionProvider(DbInfo).QuestionaireIdentification(response_id, report.Questionaire.Key);
                        report.Questionaire.Status = (Statuses)Enum.Parse(typeof(Statuses), row["quesitonaire_status"].ToString());
                        report.Questionaire.Deleted = bool.Parse(row["questionaire_deleted"].ToString());
                        report.Questionaire.Latitude = Convert.ToDouble(row["questionaire_lat"].ToString());
                        report.Questionaire.Longitude = Convert.ToDouble(row["questionaire_lng"].ToString());

                        report.FieldInspection = new FieldInspection();
                        report.FieldInspection.Key = row["inspection_key"].ToString();
                        report.FieldInspection.CreatedBy = new UserProvider(DbInfo).GetCreatedByUser(row["inspection_owner"].ToString());
                        report.FieldInspection.CreatedOn = Convert.ToDateTime(row["submitted_on"].ToString());
                        report.FieldInspection.OID = int.Parse(row["inspection_oid"].ToString());
                        report.FieldInspection.Deleted = bool.Parse(row["inspection_deleted"].ToString());
                        report.FieldInspection.Status = (Statuses)Enum.Parse(typeof(Statuses), row["inspection_status"].ToString());
                        report.FieldInspection.FieldName = row["fieldname"].ToString();
                        report.FieldInspection.Template = row["inspection_template"].ToString();
                        report.FieldInspection.Sections = new SectionProvider(DbInfo).GetSections(report.FieldInspection.Template, report.FieldInspection.Key);
                    }
                }
                
                return reports;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public FieldInspections CertificationsOverview(int configuration_Id)
        {
            FieldInspections inspections = new FieldInspections();
            try
            {
                string query = $"select * from dsto_fieldInspection where status=2 or status=3 and configuration_id='{configuration_Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        FieldInspection inspection = inspections.Add();
                        InitFieldInspection(inspection, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return inspections;
        }

        private void InitFieldInspection(FieldInspection inspection, DataRow row, string template_id = null)
        {
            try
            {
                inspection.Key = row["guid"].ToString();
                inspection.CreatedBy = row["created_by"].ToString();
                inspection.Deleted = bool.Parse(row["deleted"].ToString());
                inspection.ConfigurationId = int.Parse(row["configuration_Id"].ToString());
                inspection.OID = int.Parse(row["oid"].ToString());
                inspection.Status = (Statuses)Enum.Parse(typeof(Statuses), row["status"].ToString());
                inspection.DateTime = Convert.ToDateTime(row["datetaken"]);
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
                FieldInspection inspection = obj as FieldInspection;

                if (SaveFieldInspection(inspection))
                {
                    foreach (var section in inspection.Sections)
                    {
                        section.InspectionKey = inspection.Key;
                        sectionProvider.Save(section);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        internal bool Sync(FieldInspection inspection)
        {
            try
            {
                AnswerProvider answerProvider = new AnswerProvider(DbInfo);
                bool saved = SaveFieldInspection(inspection);
                if (saved)
                    foreach (Section section in inspection.Sections)
                    {
                        foreach (Question question in section.Questions)
                        {
                            answerProvider.Sync(inspection, question);
                            foreach (Dependency dependency in question.Dependencies)
                                foreach (Question qn in dependency.Target.SubSection.Questions)
                                    answerProvider.Sync(inspection, qn);
                        }

                        foreach (SubSection subSection in section.SubSections)
                            foreach (Question question in subSection.Questions)
                                answerProvider.Sync(inspection, question);
                    }

                return saved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveFieldInspection(FieldInspection inspection)
        {
            try
            {
                string query = string.Empty;
                var exists = RecordExists("dsto_fieldInspection", inspection.Key);
                if (!exists)
                {
                    if (!string.IsNullOrEmpty(inspection.FarmerKey) && !string.IsNullOrEmpty(inspection.Template))
                        query = $"insert into dsto_fieldInspection(guid,created_by,configuration_id,status,fieldname,datetaken,farmerid,yref_template) values('{inspection.Key}','{inspection.CreatedBy}','{inspection.ConfigurationId}',{(int)inspection.Status},'{inspection.FieldName}','{inspection.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")}','{inspection.FarmerKey}','{inspection.Template}')";
                    else
                        query = $"insert into dsto_fieldInspection(guid,created_by,configuration_id,status,fieldname,datetaken) values('{inspection.Key}','{inspection.CreatedBy}','{inspection.ConfigurationId}',{(int)inspection.Status},'{inspection.FieldName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}')";
                }
                else
                {
                    //update
                    if (!string.IsNullOrEmpty(inspection.FarmerKey) && !string.IsNullOrEmpty(inspection.Template))
                        query = $"UPDATE dsto_fieldInspection SET status='{(int)inspection.Status}', " +
                            $"farmerid='{inspection.FarmerKey}', " +
                            $"deleted='{inspection.Deleted}', " +
                            $"fieldname='{inspection.FieldName}' " +
                            $"WHERE guid='{inspection.Key}'";
                    else
                        query = $"UPDATE dsto_fieldInspection SET status='{(int)inspection.Status}', " +
                                $"created_by='{inspection.CreatedBy}', " +  
                                $"deleted='{inspection.Deleted}' " +
                                $"WHERE guid='{inspection.Key}'";
                }

                return DbInfo.ExecuteNonQuery(query) > -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteFieldInspection(int id)
        {
            string query = $"delete from dsto_fieldInspection where oid='{id}'";
            var inspection = GetInspection(id);

            foreach (var section in inspection.Sections)
            {
                new SectionProvider(DbInfo).DeleteSection(section.Key);
            }

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > -1;
        }

        public bool DeleteConfiugurationFieldInspections(int configuration_id)
        {
            try
            {
                FieldInspections inspections = GetFieldInspections(configuration_id);
                foreach (FieldInspection inspection in inspections)
                {
                    foreach (var section in inspection.Sections)
                        new SectionProvider(DbInfo).DeleteSection(section.Key);

                    string query = $"delete from dsto_fieldInspection where configuration_id='{configuration_id}'";
                    DbInfo.ExecuteNonQuery(query);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
