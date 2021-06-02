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
    public class DependencyController : BaseApiController
    {
        private ILogger<DependencyController> _logger;
        public DependencyController(ILogger<DependencyController> _logger) : base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Dependency Get(string id)
        {
            try
            {
                return new DependencyProvider(DbInfo).GetDependency(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Dependency dependency)
        {
            try
            {
                return new DependencyProvider(DbInfo).Save(dependency);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpDelete]
        public bool Delete(string id)
        {
            try
            {
                return new DependencyProvider(DbInfo).DeleteDependency(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
