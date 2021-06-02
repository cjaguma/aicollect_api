using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public static class ConfigurationHttpService
    {
        public static async Task<bool> Upload(this Configuration configuration)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Strings.BaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string content = JsonConvert.SerializeObject(configuration);
                string resourceUrl = "http://localhost:50048/api/UploadConfiguration";
                HttpResponseMessage response = await httpClient.PostAsync(resourceUrl, new StringContent(content, Encoding.Default, "application/json"));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
