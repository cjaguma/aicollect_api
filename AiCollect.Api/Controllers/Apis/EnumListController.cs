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
    public class EnumListController : BaseApiController
    {
        private ILogger<EnumListController> _logger;
        public EnumListController(ILogger<EnumListController> _logger):base()
        {
            this._logger = _logger;
        }

        [HttpGet("prices/configuration/{id}")]
        public EnumList GetPrices(string id)
        {
            try
            {
                return new EnumListProvider(DbInfo).GetEnumList(int.Parse(id), EnumListTypes.Price);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("regions/configuration/{id}")]
        public EnumList GetRegions(string id)
        {
            try
            {
                return new EnumListProvider(DbInfo).GetEnumList(int.Parse(id), EnumListTypes.Region);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("products/configuration/{id}")]
        public EnumList GetProducts(string id)
        {
            try
            {
                return new EnumListProvider(DbInfo).GetEnumList(int.Parse(id), EnumListTypes.Product);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(EnumList enumList)
        {
            try
            {
                return new EnumListProvider(DbInfo).Save(enumList);
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
                return new EnumListProvider(DbInfo).DeleteEnumList(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

    }
}
