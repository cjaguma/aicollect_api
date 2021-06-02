using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Data.Services
{
    public class MailService
    {
        private static string Email = "noreply@aicollectapp.com";
        private static string SENDGRID_API_KEY = "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";

        private static readonly HttpClient _client = new HttpClient();

        public MailService()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SENDGRID_API_KEY);
        }

        private static string MailBody(string innerHtml, string url)
        {
            try
            {
                string body = "<div style='background: #f5f4f4;padding: 100px;'>"
                            + " <div style='background: #fff;width:100%;box-shadow: 2px, 3px, 3px 3px #ccc !important;'>"
                            + "     <div style='background: #051bb9;padding: 30px;'>"
                            + "         <h1 style='margin: auto; text-align: center; color: #fff'> AiCollect </h1>"
                            + "     </div>"
                            + "     <div style='padding: 30px;'>"
                            + innerHtml 
                            + "     </div>"
                            + "     <hr style='background: #051bb9; height: 10px;'>"
                            + "     <div style='padding: 20px; text-align: center;'>"
                            + "         Have questions or need help? Email <a href='#'> support@aicollectapp.com </a> or call 0414672152"
                            + "     </div>"
                            + "     <hr style='background: #ea5cab; height: 10px;margin-bottom: 0px;'>"
                            + "     <div style='text-align: center;padding: 20px; background: #E4E0E0;'>"
                            + "         Plot 96B Bukoto Street Kamwokya"
                            + "     </div>"
                            + " </div>"
                            + "</div>";
                return body;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async static Task SendMail(string recipient, bool IsHtml, string Subject, string innerHtml, string url = null)
        {
            try
            {
                Environment.SetEnvironmentVariable("SENDGRID_API_KEY", SENDGRID_API_KEY);
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(Email, "AICollect");
                var subject = Subject;
                var to = new EmailAddress(recipient);
                var htmlContent = MailBody(innerHtml, url);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
        }


        public class Personalization
        {
            public List<Mail> To { get; set; }
        }

        public class Mail
        {
            public string Email { get; set; }
        }

        public class Message
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public class Construct
        {
            public List<Personalization> Personalizations { get; set; }
            public Mail From { get; set; }
            public string Subject { get; set; }
            public List<Message> Content { get; set; }
        }
    }
}
