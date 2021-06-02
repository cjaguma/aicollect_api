using AiCollect.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using AiCollect.Data;
using Microsoft.AspNetCore.Mvc;
using AiCollect.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class ClientsController : BaseApiController
    {
        private ILogger<ClientsController> _logger;
        public ClientsController(ILogger<ClientsController> _logger)
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public Client Get(int id)
        {
            try
            {
                return new ClientProvider(DbInfo).GetClient(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet]
        public Clients Get()
        {
            try
            {
                return new ClientProvider(DbInfo).GetClients();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Client client)
        {
            try
            {
                return new ClientProvider(DbInfo).Save(client);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("update")]
        public bool Update(Client client)
        {
            try
            {
                return new ClientProvider(DbInfo).Save(client);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            try
            {
                return new ClientProvider(DbInfo).Delete(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
