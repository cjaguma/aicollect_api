using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AiCollect.Api.Controllers
{
    [Route("")]
    [Produces("text/html")]
    public class StarterController : BaseApiController
    {
        private readonly ILogger<StarterController> _logger;

        public StarterController(ILogger<StarterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ContentResult Get()
        {
            try
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = "<html>" +
                              "      <head>" +
                              "          <title> AiCollect Api </title>" +
                              "      <head>" +
                              "      <body>" +
                              "         <style>" +
                              "              div" +
                              "              {" +
                              "                  padding-top: 200px;" +
                              "                  text-align: center;" +
                              "                  font-family: cursive;" +
                              "              }" +
                              "         </style>" +
                              "         <div>" +
                              "              <h1>AiCollect Api</h1>" +
                              "              <label> See <a href='/swagger'> Documentation </a ></label>" +
                              "         </div>" +
                              "     </body>" +
                              "</html>"
                };
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
