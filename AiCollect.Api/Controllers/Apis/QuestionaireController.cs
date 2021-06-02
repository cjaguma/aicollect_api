using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using AiCollect.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class QuestionaireController : BaseApiController
    {
        private ILogger<QuestionaireController> _logger;
        public QuestionaireController(ILogger<QuestionaireController> _logger)
        {
            this._logger = _logger;
        }
    
        [HttpPost]
        public bool Post(Questionaire questionaire)
        {
            try
            {
                return new QuestionaireProvider(DbInfo).Save(questionaire);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("{id}")]
        public Questionaire Get(int id)
        {
            try
            {
                return new QuestionaireProvider(DbInfo).GetQuestionaire(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("templates")]
        public Questionaires Templates()
        {
            try
            {
                return new QuestionaireProvider(DbInfo).GetQuestionaires(0);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("configuration/{id}")]
        public Questionaires ConfigurationQuestionaires(int id)
        {
            try
            {
                return new QuestionaireProvider(DbInfo).GetQuestionaires(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("overview/{id}")]
        public Questionaires ReviewQuestionaires(string id)
        {
            try
            {
                return new QuestionaireProvider(DbInfo).GetReviewQuestionaires(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpDelete]
        public bool Delete(string key)
        {
            try
            {
                return new QuestionaireProvider(DbInfo).DeleteQuestionaire(key);

            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
