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
    public class CertificationProvider : Provider
    {
        public CertificationProvider(dloDbInfo dbInfo) : base(dbInfo)
        {

        }

        public Certification GetCertification(int id, string template_id = null)
        {
            string query = $"select * from dsto_certification where oid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                var certificationType = (CertificationTypes)Convert.ToInt32(row["CertificationType"]);
                Certification certification = ObjectFactory.CreateCertification(certificationType, null);
                InitCertification(certification, row);
                certification.Sections = new SectionProvider(DbInfo).GetSections(certification.Key, template_id);

                return certification;
            }
            return null;
        }

        public Certification GetCertification(string id, string template_id = null)
        {
            string query = $"select * from dsto_certification where guid = '{id}'";
            var table = DbInfo.ExecuteSelectQuery(query);
            if (table.Rows.Count > 0 && table.Rows.Count == 1)
            {
                DataRow row = table.Rows[0];
                var certificationType = (CertificationTypes)Convert.ToInt32(row["CertificationType"]);
                Certification certification = ObjectFactory.CreateCertification(certificationType, null);
                InitCertification(certification, row);
                certification.Sections = new SectionProvider(DbInfo).GetSections(certification.Key, template_id);

                return certification;
            }
            return null;
        }

        public Reports GetReports(string response_id)
        {
            try
            {
                Reports reports = new Reports();
                string query = $"select dsto_questionaire.Longitude as questionaire_lng, dsto_questionaire.Latitude as questionaire_lat, " +
                    $"dsto_questionaire.deleted as questionaire_deleted, dsto_questionaire.Status as quesitonaire_status, " +
                    $"dsto_questionaire.Name as questionaire_name, dsto_questionaire.oid as questionaire_oid, " +
                    $"dsto_questionaire.guid as questionaire_guid, dsto_certification.guid as certification_key, " +
                    $"dsto_certification.created_by as certification_owner, dsto_certification.oid as certification_oid, " +
                    $"dsto_certification.deleted as certification_deleted, dsto_certification.status as certification_status, dsto_certification.CertificationType as certificationType, " +
                    $"dsto_certification.name as certification_name, dsto_certification.yref_template as certification_template " +
                    $"from dsto_certification inner join dsto_Questionaire on dsto_certification.farmerid = dsto_Questionaire.guid " +
                    $"where dsto_questionaire.yref_template='{response_id}'"; ;
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

                        report.Certification = new Certifications(null).Add((CertificationTypes)Enum.Parse(typeof(CertificationTypes), row["certificationType"].ToString()));
                        report.Certification.Key = row["certification_key"].ToString();
                        report.Certification.CreatedBy = new UserProvider(DbInfo).GetCreatedByUser(row["certification_owner"].ToString());
                        report.Certification.OID = int.Parse(row["certification_oid"].ToString());
                        report.Certification.Deleted = bool.Parse(row["certification_deleted"].ToString());
                        report.Certification.Status = (Statuses)Enum.Parse(typeof(Statuses), row["certification_status"].ToString());
                        report.Certification.Name = row["certification_name"].ToString();
                        report.Certification.Template = row["certification_template"].ToString();
                        report.Certification.Sections = new SectionProvider(DbInfo).GetSections(report.Certification.Template, report.Certification.Key);
                    }
                }
                return reports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool Sync(Certification certification)
        {
            try
            {
                AnswerProvider answerProvider = new AnswerProvider(DbInfo);
                bool saved = SaveCertification(certification);
                if (saved)
                    foreach (Section section in certification.Sections)
                    {
                        foreach (Question question in section.Questions)
                        {
                            answerProvider.Sync(certification, question);
                            foreach (Dependency dependency in question.Dependencies)
                                foreach (Question qn in dependency.Target.SubSection.Questions)
                                    answerProvider.Sync(certification, qn);
                        }

                        foreach (SubSection subSection in section.SubSections)
                            foreach (Question question in subSection.Questions)
                                answerProvider.Sync(certification, question);
                    }

                return saved;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(int id)
        {
            string query = $"delete from dsto_certification where oid='{id}'";
            Certification certification = GetCertification(id);

            foreach (var s in certification.Sections)
            {
                new SectionProvider(DbInfo).DeleteSection(s.Key);
            }

            var rows = DbInfo.ExecuteNonQuery(query);
            return rows > 0;
        }

        //public bool Delete(string reference)
        //{
        //    string query = $"delete from dsto_sections where yref_questionaire = '{reference}' or yref_field_inspection = '{reference}' or yref_certification = '{reference}'";
        //    var rows = DbInfo.ExecuteNonQuery(query);
        //    return rows > 0;
        //}



        public Certifications GetCertifications(int configuration_Id, bool withTransaction = false)
        {
            Certifications certifications = new Certifications(null);
            try
            {
                string query = $"select * from dsto_certification where status=0 and configuration_id='{configuration_Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var certificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), row["CertificationType"].ToString());
                        Certification certification = certifications.Add(certificationType);
                        InitCertification(certification, row);
                        certification.Sections = new SectionProvider(DbInfo).GetSections(certification.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return certifications;
        }


        public Certifications ConfigurationCertifications(int configuration_Id)
        {
            Certifications certifications = new Certifications(null);
            try
            {
                string query = $"select * from dsto_certification where status=0 and configuration_id='{configuration_Id}' and deleted=false";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var certificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), row["CertificationType"].ToString());
                        Certification certification = certifications.Add(certificationType);
                        InitCertification(certification, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return certifications;
        }

        public Certifications CertificationsOverview(int configuration_Id)
        {

            Certifications certifications = new Certifications(null);
            try
            {
                string query = $"select * from dsto_certification where status=2 or status=3 and configuration_id='{configuration_Id}'";
                var table = DbInfo.ExecuteSelectQuery(query);
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var certificationType = (CertificationTypes)Enum.Parse(typeof(CertificationTypes), row["CertificationType"].ToString());
                        Certification certification = certifications.Add(certificationType);
                        InitCertification(certification, row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return certifications;
        }

        private void InitCertification(Certification certification, DataRow row)
        {
            try
            {
                certification.Key = row["guid"].ToString();
                certification.CreatedBy = row["created_by"].ToString();
                certification.OID = int.Parse(row["oid"].ToString());
                certification.Deleted = bool.Parse(row["deleted"].ToString());
                certification.Status = (Statuses)Enum.Parse(typeof(Statuses), row["status"].ToString());
                certification.Name = row["name"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveCertification(Certification certification)
        {
            try
            {
                var exists = RecordExists("dsto_certification", certification.Key);

                string query = string.Empty;
                if (!exists)
                {
                    if (!string.IsNullOrEmpty(certification.FarmerKey) && !string.IsNullOrEmpty(certification.Template))
                        query = $"insert into dsto_certification(guid,Name,created_by,farmerid,CertificationType,status,configuration_id,yref_template) values('{certification.Key}','{certification.Name}','{certification.CreatedBy}','{certification.FarmerKey}',{(int)certification.CerificationType},'{(int)certification.Status}','{certification.ConfigurationId}','{certification.Template}')";
                    else
                        query = $"insert into dsto_certification(guid,Name,created_by,CertificationType,status,configuration_id) values('{certification.Key}','{certification.Name}','{certification.CreatedBy}',{(int)certification.CerificationType},'{(int)certification.Status}','{certification.ConfigurationId}')";
                }
                else
                {
                    //update
                    query = $"UPDATE dsto_certification SET " +
                            $"Name='{certification.Name}', " +
                            $"Deleted='{certification.Deleted}', " +
                            $"CertificationType='{(int)certification.CerificationType}' " +
                            $"WHERE guid='{certification.Key}'";
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
                Certification certification = obj as Certification;

                if (SaveCertification(certification))
                {
                    foreach (var section in certification.Sections)
                    {
                        section.CertificationKey = certification.Key;
                        sectionProvider.Save(section);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
