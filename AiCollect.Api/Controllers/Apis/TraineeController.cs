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
    public class TraineeController : BaseApiController
    {
        private ILogger<TraineeController> _logger;
        public TraineeController(ILogger<TraineeController> _logger) : base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Trainee Get(string id)
        {
            try
            {
                return new TraineeProvider(DbInfo).GetTrainee(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("training/{training_id}")]
        public Trainees GetTrainingTrainees(string training_id)
        {
            try
            {
                return new TraineeProvider(DbInfo).GetTrainees(training_id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Trainee trainee)
        {
            try
            {
                var provider = new TraineeProvider(DbInfo);
                return provider.Save(trainee);
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
                return new TraineeProvider(DbInfo).DeleteTrainee(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
