using AiCollect.Api.Services;
using AiCollect.Core;
using Google.Cloud.Translation.V2;
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
    public class TranslationController : BaseApiController
    {
        private ILogger<TranslationController> _logger;
        public TranslationController(ILogger<TranslationController> _logger)
        {
            this._logger = _logger;
        }

        [HttpPost]
        public Configuration Translate(Configuration configuration)
        {
            try
            {
                new TranslationService(configuration).Translate();
                return configuration;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
