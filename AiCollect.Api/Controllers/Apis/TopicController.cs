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
    public class TopicController : BaseApiController
    {
        private ILogger<TopicController> _logger;
        public TopicController(ILogger<TopicController> _logger) :base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Topic Get(string id)
        {
            try
            {
                return new TopicProvider(DbInfo).GetTopic(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("training/{id}")]
        public Topics GetTopics(string id)
        {
            try
            {
                return new TopicProvider(DbInfo).GetTopics(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Topic topic)
        {
            try
            {
                return new TopicProvider(DbInfo).Save(topic);
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
                return new TopicProvider(DbInfo).DeleteTopic(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
