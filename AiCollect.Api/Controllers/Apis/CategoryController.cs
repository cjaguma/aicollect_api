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
    public class CategoryController : BaseApiController
    {
        private ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> _logger) :base()
        {
            this._logger = _logger;
        }

        [HttpGet]
        public Categories Get()
        {
            try
            {
                return new CategoryProvider(DbInfo).RetrieveCategories();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("templates")]
        public Categories GetTemplateCategories()
        {
            try
            {
                return new CategoryProvider(DbInfo).RetrieveTemplateCategories();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Category category)
        {
            try
            {
                return new CategoryProvider(DbInfo).Save(category);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
