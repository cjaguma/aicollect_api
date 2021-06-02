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
    public class InspectionController : BaseApiController
    {
        private ILogger<InspectionController> _logger;
        public InspectionController(ILogger<InspectionController> _logger) : base()
        {
            this._logger = _logger;
        }

        [HttpGet("{id}")]
        public FieldInspection Get(string id)
        {
            try
            {
                return new FieldInspectionProvider(DbInfo).GetInspection(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("configuration/{id}")]
        public FieldInspections GetCertifications(string id)
        {
            try
            {
                return new FieldInspectionProvider(DbInfo).GetFieldInspections(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("reports/{response_id}")]
        public Reports GetReports(string response_id)
        {
            try
            {
                return new FieldInspectionProvider(DbInfo).GetReports(response_id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("overview/{id}")]
        public FieldInspections CertificationsOverview(string id)
        {
            try
            {
                return new FieldInspectionProvider(DbInfo).CertificationsOverview(int.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(FieldInspection inspection)
        {
            try
            {
                return new FieldInspectionProvider(DbInfo).Save(inspection);
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
                return new FieldInspectionProvider(DbInfo).DeleteFieldInspection(id);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}