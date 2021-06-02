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
    public class UserRightsController : BaseApiController
    {
        private ILogger<UserRightsController> _logger;
        public UserRightsController(ILogger<UserRightsController> _logger) : base()
        {
            this._logger = _logger;
        }
      
        [HttpPost]
        public HttpResponseMessage Post(Configuration configuration)
        {
            try
            {
                configuration.InitUserRights();
                return CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
