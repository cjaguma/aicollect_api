using AiCollect.Data;
using AiCollect.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using Configuration = System.Configuration.Configuration;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class BaseApiController : ControllerBase
    {
        public dloDbInfo DbInfo { get; set; }
        public IConfiguration Configuration;

        public BaseApiController()
        {
            Init();
        }

        private void Init()
        {
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                DbInfo = new AiCollect.Data.dloDbInfo();
                DbInfo.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            }
            catch(Exception ex)
            {

            }
        }

        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode)
        {
            return new HttpResponseMessage(statusCode);
        }

        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode, object data)
        {
            return data == null ? CreateResponse(statusCode, data) : CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildErrorResult(HttpStatusCode statusCode, string errorCode = null, string message = null)
        {
            return CreateResponse(statusCode, new Error()
            {
                ErrorCode = errorCode,
                Message = message
            });
        }

        public static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object content = null) 
        {
            if (content == null)
                return new HttpResponseMessage(statusCode);
            else
                return new HttpResponseMessage()
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(content))
                };
        }
    }

    public class Error
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
