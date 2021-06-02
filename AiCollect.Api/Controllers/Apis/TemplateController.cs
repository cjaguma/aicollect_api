using AiCollect.Data.Providers;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class TemplateController : BaseApiController
    {
        private ILogger<TemplateController> _logger;
        public TemplateController(ILogger<TemplateController> _logger) : base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Template Get(string id)
        {
            try
            {
                return new TemplateProvider(DbInfo).RetrieveTemplate(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet]
        public Templates Get()
        {
            try
            {
                return new TemplateProvider(DbInfo).RetrieveTemplates();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Template template)
        {
            try
            {
                return new TemplateProvider(DbInfo).Save(template);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

    }
}
