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
    public class CertificationController : BaseApiController
    {
        private ILogger<CertificationController> _logger;
        public CertificationController(ILogger<CertificationController> _logger) :base()
        {
            this._logger = _logger;
        }

        [HttpGet("configuration/{Id}")]
        public Certifications GetCertifications(string id)
        {
            try
            {
                return new CertificationProvider(DbInfo).GetCertifications(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("overview/{Id}")]
        public Certifications CertificationsOverview(string id)
        {
            try
            {
                return new CertificationProvider(DbInfo).CertificationsOverview(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("reports/{respons_id}")]
        public Reports GetReports(string respons_id)
        {
            try
            {
                return new CertificationProvider(DbInfo).GetReports(respons_id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("{id}")]
        public Certification Get(string id)
        {
            try
            {
                return new CertificationProvider(DbInfo).GetCertification(int.Parse(id));
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(Certification certification)
        {
            try
            {
                return new CertificationProvider(DbInfo).Save(certification);
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
        public bool Delete(int id)
        {
            try
            {
                return new CertificationProvider(DbInfo).Delete(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
