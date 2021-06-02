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
    public class TrainerController : BaseApiController
    {
        private ILogger<TrainerController> _logger;
        public TrainerController(ILogger<TrainerController> _logger):base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Trainer Get(string id)
        {
            try
            {
                return new TrainerProvider(DbInfo).GetTrainer(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("training/{id}")]
        public Trainers GetTrainers(string id)
        {
            try
            {
                return new TrainerProvider(DbInfo).GetTrainers(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Trainer trainer)
        {
            try
            {
                return new TrainerProvider(DbInfo).Save(trainer);
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
                return new TrainerProvider(DbInfo).DeleteTrainer(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
