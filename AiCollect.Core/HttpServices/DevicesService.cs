using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core.HttpServices
{
    public static class DevicesService
    {

        public static async Task<bool> Activate(this Device device)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Strings.BaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string content = JsonConvert.SerializeObject(device);
                string resourceUrl = $"{Strings.BaseUrl}/Device/Activate?id={device.Key}&activate=true";             
                HttpResponseMessage response = await httpClient.GetAsync(resourceUrl);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
