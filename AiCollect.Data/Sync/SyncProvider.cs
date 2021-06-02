using AiCollect.Data;
using AiCollect.Data.Providers;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Providers
{
    public class SyncProvider : Provider
    {

        public SyncProvider(dloDbInfo dbInfo) : base(dbInfo)
        {
        }

        public bool SyncCertification(Certification certification)
        {
            try
            {
                CertificationProvider certificationProvider = new CertificationProvider(DbInfo);
                return certificationProvider.Sync(certification);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SyncTraining(Training training)
        {
            try
            {
                TrainingProvider trainingProvider = new TrainingProvider(DbInfo);
                return trainingProvider.Save(training);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SyncEnumLists(EnumLists enumListsIn)
        {
            try
            {
                EnumListProvider enumListProvider = new EnumListProvider(DbInfo);
                foreach (var enumList in enumListsIn)
                {
                    enumListProvider.Save(enumList);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SyncPurchase(Purchase purchase)
        {
            try
            {
                PurchaseProvider purchaseProvider = new PurchaseProvider(DbInfo);
                return purchaseProvider.Save(purchase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SyncFieldInspection(FieldInspection inspection)
        {
            try
            {
                FieldInspectionProvider inspectionProvider = new FieldInspectionProvider(DbInfo);
                return inspectionProvider.Sync(inspection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool SyncQuestionaire(Questionaire Questionaire)
        {
            try
            {
                QuestionaireProvider questionaireProvider = new QuestionaireProvider(DbInfo);
                return questionaireProvider.Sync(Questionaire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CompareQuestionaire(Questionaire questionaireIn, Questionaire questionaireInDb)
        {
            foreach (var section in questionaireIn.Sections)
            {
                var sectionIn = questionaireInDb.Sections.ByKey(section.Key);
                var exists = sectionIn != null;
                if (exists)
                {
                    //update
                    new SectionProvider(DbInfo).Save(section);
                }
                else
                {
                    new SectionProvider(DbInfo).Save(section);
                }
            }
        }
    }
}
