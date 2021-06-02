using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Translation.V2;
using Google.Apis.Translate.v3;

namespace AiCollect.Api.Services
{
    public class TranslationHandler
    {

        public TranslationHandler()
        {
            DoTranslation();
        }

        private void DoTranslation()
        {
         
         
            string credential_path = System.IO.Path.Combine(Environment.CurrentDirectory, "Credentials", "google_translate.json");
            if (System.IO.File.Exists(credential_path))
            {
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
                var translationClient = TranslationClient.Create();
                var translationResult = translationClient.TranslateText("Work", "fr");
                var t = translationResult.TranslatedText;
            }
           
        }
    }
}