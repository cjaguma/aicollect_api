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
using System.Web;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class PackageController : BaseApiController
    {
        private ILogger<PackageController> _logger;
        public PackageController(ILogger<PackageController> _logger)
        {
            this._logger = _logger;
        }

        [HttpGet]
        public Packages Get()
        {
            try
            {
                return new PackageProvider(DbInfo).RetrievePackages();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Package package)
        {
            try
            {
                return new PackageProvider(DbInfo).Save(package);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
