using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public static class LoginService
    {
        public static async Task<bool> LogIn(this User user)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(Strings.BaseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));            
                string resourceUrl = $"http://localhost:50048/api/User/Login?username={user.UserName}&password={user.Password}";
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
