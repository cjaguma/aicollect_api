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
    public class SyncController : BaseApiController
    {
        private ILogger<SyncController> _logger;
        public SyncController(ILogger<SyncController> _logger) :base()
        {
            this._logger = _logger;
        }

        [HttpPost("questionaire")]
        public bool Post(Questionaire questionaire)
        {
            try
            {
                return new SyncProvider(DbInfo).SyncQuestionaire(questionaire);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("inspection")]
        public bool Post(FieldInspection inspection)
        {
            try
            {
                return new SyncProvider(DbInfo).SyncFieldInspection(inspection);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("certification")]
        public bool Post(Certification certification)
        {
            try
            {
                return new SyncProvider(DbInfo).SyncCertification(certification);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("training")]
        public bool Post(Training training)
        {
            try
            {
                return new SyncProvider(DbInfo).SyncTraining(training);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("certification")]
        public bool Post(Purchase purchase)
        {
            try
            {
                return new SyncProvider(DbInfo).SyncPurchase(purchase);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
