using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AiCollect.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using AiCollect.Data.Providers;
using AiCollect.Core.Collections;
using Microsoft.Extensions.Logging;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class ConfigurationController : BaseApiController
    {
        private ILogger<ConfigurationController> _logger;
        public ConfigurationController(ILogger<ConfigurationController> _logger)
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Configuration Get(int id)
        {
            try
            {
                return new ConfigurationProvider(DbInfo).GetConfiguration(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("client/{id}")]
        public Configurations ClientConfigurations(int id)
        {
            try
            {
                return new ConfigurationProvider(DbInfo).ClientConfigurations(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("client/mobile/{id}")]
        public Configurations ClientMobileConfigurations(int id)
        {
            try
            {
                return new ConfigurationProvider(DbInfo).ClientMobileConfigurations(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public object Post(Configuration configuration)
        {
            try
            {
                var provider = new ConfigurationProvider(DbInfo);
                var _save =  provider.Save(configuration);

                if(_save)
                    return  provider.GetConfiguration(configuration.Key);

                return _save;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPut]
        public void Put(Configuration configuration)
        {
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            try
            {
                return new ConfigurationProvider(DbInfo).DeleteConfiguration(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
