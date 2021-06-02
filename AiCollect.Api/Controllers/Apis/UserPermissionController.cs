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
    public class UserPermissionController : BaseApiController
    {
        private ILogger<UserPermissionController> _logger;
        public UserPermissionController(ILogger<UserPermissionController> _logger):base()
        {
            this._logger = _logger;
        }

        [HttpPost]
        public bool Post(UserPermissions userPermissions)
        {
            try
            {
                UserPermissionProvider provider = new UserPermissionProvider(DbInfo);
                foreach (var userPermission in userPermissions)
                {
                    provider.Save(userPermission);
                }
                return true;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

    }
}
