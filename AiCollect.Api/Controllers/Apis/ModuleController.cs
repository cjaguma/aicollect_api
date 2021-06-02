using AiCollect.Data;
using AiCollect.Data.Providers;
using AiCollect.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class ModuleController : BaseApiController
    {
        private ILogger<ModuleController> _logger;
        public ModuleController(ILogger<ModuleController> _logger):base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}/{clientName}")]
        public Module Get(int id, string clientName)
        {
            try
            {
                DbInfo.Database = clientName;
                return new ModuleProvider(DbInfo).RetrieveModule(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("{configurationId}/{clientName}")]
        public Modules GetConfigurationModules(int configurationId, string clientName)
        {
            try
            {
                DbInfo.Database = clientName;
                return new ModuleProvider(DbInfo).RetrieveModules(configurationId);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Module module)
        {
            try
            {
                DbInfo.Database = module.ClientName;
                return new ModuleProvider(DbInfo).Save(module);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        public bool Delete(string id)
        {
            try
            {
                return new ModuleProvider(DbInfo).SoftDelete("dsto_module", id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
