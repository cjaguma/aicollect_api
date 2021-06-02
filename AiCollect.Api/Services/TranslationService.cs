
using AiCollect.Core;
using Google.Cloud.Translation.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AiCollect.Api.Services
{
    public class TranslationService
    {
        #region Members
        private User User { get; set; }
        private Configuration _configuration;
        private TranslationClient _translationClient;
        #endregion
        #region Constructor
        public TranslationService(Configuration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Methods

        #region Translate
        internal void Translate()
        {
            string credential_path = System.IO.Path.Combine(Environment.CurrentDirectory, "Credentials", "Aicollect-b73d9abcb323.json");
            if (System.IO.File.Exists(credential_path))
            {
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
                _translationClient = TranslationClient.Create();
                TranslateConfiguration();
            }
        }
        #endregion

        #region TranslateConfiguration
        private void TranslateConfiguration()
        {

            TranslateQuestionaires();
            TranslateEumerationLists();
            TranslateUserRights();
            TranslateTrainings();
            TranslateCertifications();
            TranslateInspections();
        }
        #endregion

        #region TranslateQuestionaires
        private void TranslateQuestionaires()
        {
            foreach (var questionaire in _configuration.Questionaires)
            {
                AddTranslation(questionaire.Name);
                foreach (var section in questionaire.Sections)
                {
                    AddTranslation(section.Name);
                    foreach (var qn in section.Questions)
                    {
                        AddTranslation(qn.Name);
                        AddTranslation(qn.QuestionText);
                        foreach (var sb in section.SubSections)
                        {
                            AddTranslation(sb.Name);
                            foreach (var qnSb in sb.Questions)
                            {
                                AddTranslation(qnSb.Name);
                                AddTranslation(qnSb.QuestionText);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Translate EnumerationLists
        private void TranslateEumerationLists()
        {
            foreach (var enumList in _configuration.EnumerationLists)
            {
                AddTranslation(enumList.Name);
                foreach (var enumValue in enumList.EnumValues)
                {
                    AddTranslation(enumValue.Description);
                    AddTranslation(enumValue.Code.ToString());
                }
            }
        }

        #endregion

        #region AddUserRights
        private void TranslateUserRights()
        {
            foreach (var userRight in _configuration.UserRights)
            {
                AddTranslation(userRight.ObjectName);
            }
        }
        #endregion

        #region Translate Certifications

        private void TranslateCertifications()
        {
            foreach (var certification in _configuration.Certifications)
            {
                AddTranslation(certification.Name);
                foreach (var section in certification.Sections)
                {
                    AddTranslation(section.Name);
                    foreach (var qn in section.Questions)
                    {
                        AddTranslation(qn.Name);
                        AddTranslation(qn.QuestionText);
                        foreach (var subSection in section.SubSections)
                        {
                            AddTranslation(subSection.Name);
                            foreach (var qn1 in subSection.Questions)
                            {
                                AddTranslation(qn1.Name);
                                AddTranslation(qn1.QuestionText);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Translate Trainings

        private void TranslateTrainings()
        {
            foreach (var training in _configuration.Trainings)
            {
                AddTranslation(training.Name);
                foreach (var trainee in training.Trainees)
                {
                    AddTranslation(trainee.FarmerKey);
                }
                foreach (var topic in training.Topics)
                {
                    AddTranslation(topic.Name);
                }
                foreach (var trainer in training.Trainers)
                {
                    AddTranslation(trainer.Name);
                }
            }
        }

        #endregion

        #region Translate Trainings

        private void TranslateInspections()
        {
            foreach (var inspection in _configuration.Inspections)
            {
                AddTranslation(inspection.FieldName);
                foreach (var section in inspection.Sections)
                {
                    AddTranslation(section.Name);
                    foreach (var qn in section.Questions)
                    {
                        AddTranslation(qn.Name);
                        AddTranslation(qn.QuestionText);
                        foreach (var subSection in section.SubSections)
                        {
                            AddTranslation(subSection.Name);
                            foreach (var qn1 in subSection.Questions)
                            {
                                AddTranslation(qn1.Name);
                                AddTranslation(qn1.QuestionText);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region AddTranslation
        private void AddTranslation(string originalText)
        {
            if (string.IsNullOrWhiteSpace(originalText)) return;
            Translatable translatable = _configuration.Translatables.Add();
            translatable.Name = originalText;
            foreach (var language in _configuration.Languages)
            {
                var translatedResult = _translationClient.TranslateText($"{originalText}", $"{language.Code}");
                Translation translation = translatable.Translations.Add();
                translation.Language = language;
                translation.TranslatedText = translatedResult.TranslatedText;
            }
        }
        #endregion



        #endregion

    }
}