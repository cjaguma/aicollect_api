using AiCollect.Data.Providers;
using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Google.Apis.Translate.v3;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Mvc;
using AiCollect.Core.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class BillingController : BaseApiController
    {
        public readonly ILogger<BillingController> _logger;
        public BillingController(ILogger<BillingController> _logger) :base()
        {
            this._logger = _logger;
        }

     
        [HttpGet("client/{id}")]
        public Billings Get(string id)
        {
            try
            {
                return new BillingProvider(DbInfo).RetrieveClientBills(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet]
        public Billings Get()
        {
            try
            {
                return new BillingProvider(DbInfo).RetrieveBills();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Billing billing)
        {
            try
            {
                return new BillingProvider(DbInfo).Save(billing);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

    }
}
