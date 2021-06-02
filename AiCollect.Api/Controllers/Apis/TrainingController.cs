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
    public class TrainingController : BaseApiController
    {
        private ILogger<TrainingController> _logger;
        public TrainingController(ILogger<TrainingController> _logger):base()
        {
            this._logger = _logger;
        }
  
        [HttpGet("{id}")]
        public Training Get(string id)
        {
            try
            {
                return new TrainingProvider(DbInfo).GetTraining(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("reports/{configuration_id}")]
        public Reports GetReports(string configuration_id)
        {
            try
            {
                return new TrainingProvider(DbInfo).GetReports(configuration_id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("configuration/{id}")]
        public Trainings GetTrainings(string id)
        {
            try
            {
                return new TrainingProvider(DbInfo).GetTrainings(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public object Post(Training training)
        {
            try
            {
                var provider = new TrainingProvider(DbInfo);
                var isSaved = provider.Save(training);
                if (isSaved)
                    return provider.LastEntry(training.Key);

                return isSaved;
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
                return new TrainingProvider(DbInfo).DeleteTraining(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
