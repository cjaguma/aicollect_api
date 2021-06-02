using AiCollect.Core;
using AiCollect.Api.Providers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class UploadConfigurationController : BaseApiController
    {
        private ILogger<UploadConfigurationController> _logger;
        public string GetStringQuery
        {
            get
            {
                return "SELECT TOP 1 [config] FROM [dsto_configuration] )";
            }
        }

        public UploadConfigurationController(ILogger<UploadConfigurationController> _logger)
        {
            this._logger = _logger;
        }

        // GET: api/UploadConfiguration
        public string Get(string database)
        {
            ConfigurationProvider provider = new ConfigurationProvider();
            provider._dbClass.Database = database;
            return provider.GetConfigurationString(GetStringQuery);
        }

        // GET: api/UploadConfiguration/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UploadConfiguration
        [HttpPost]
        public HttpResponseMessage Post(Configuration configuration)
        {
            DoSync(configuration);
            return CreateResponse(HttpStatusCode.OK);
        }

        private void DoSync(Configuration configuration)
        {
            ConfigurationProvider provider = new ConfigurationProvider();
            provider._dbClass.Database = configuration.DbInfo.Database;
            bool saved = false;
            //first create database if missing
            var dbExists = provider._dbClass.DatabaseExists(configuration.DbInfo.Database);
            if (!dbExists)
            {
                provider._dbClass.CreateDatabase(configuration);
                saved = provider.SaveConfiguration(configuration);
            }
            else
            {
                //get server configuration
                var configString = provider.GetConfigurationString(configuration.GetStringQuery);
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
                jsonSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonSettings.TypeNameHandling = TypeNameHandling.Auto;

                Configuration serverConfiguration = Core.Configuration.LoadJson(configString, jsonSettings);

                //compare
                var different = configuration.CompareTo(serverConfiguration) == 0;
                if(different)
                {
                    //upgrade stuff

                }

            }
        }

        private void GetServerConfiguration()
        {

        }


        // PUT: api/UploadConfiguration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UploadConfiguration/5
        public void Delete(int id)
        {
        }

    }
}
