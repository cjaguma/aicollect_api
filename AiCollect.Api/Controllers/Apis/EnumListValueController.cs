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
    public class EnumListValueController : BaseApiController
    {
        private ILogger<EnumListValueController> _logger;
        public EnumListValueController(ILogger<EnumListValueController> _logger):base()
        {
            this._logger = _logger;
        }

        [HttpGet("{key}")]
        public EnumListValues Get(string key)
        {
            try
            {
                return new EnumListValueProvider(DbInfo).GetEnumListValue(key);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(EnumListValue enumList)
        {
            try
            {
                return new EnumListValueProvider(DbInfo).Save(enumList);
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
                return new EnumListValueProvider(DbInfo).DeleteEnumValue(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
