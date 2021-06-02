using AiCollect.Data;
using AiCollect.Data.Providers;
using AiCollect.Core;
using AiCollect.Core.Collections;
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
    public class PurchaseController : BaseApiController
    {
        private ILogger<TopicController> _logger;
        public PurchaseController(ILogger<TopicController> _logger) : base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Purchase Get(string id)
        {
            try
            {
                return new PurchaseProvider(DbInfo).GetPurchase(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("configuration/{id}")]
        public Purchases GetPurchase(string id)
        {
            try
            {
                return new PurchaseProvider(DbInfo).GetPurchases(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("reports/{response_id}")]
        public Reports GetReports(string response_id)
        {
            try
            {
                return new PurchaseProvider(DbInfo).GetReports(response_id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Purchase purchase)
        {
            try
            {
                return new PurchaseProvider(DbInfo).Save(purchase);
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
        public bool Delete(int id)
        {
            try
            {
                return new PurchaseProvider(DbInfo).DeletePurchase(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
