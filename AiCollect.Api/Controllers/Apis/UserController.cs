using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AiCollect.Data;
using System.Configuration;
using AiCollect.Core;
using Microsoft.AspNetCore.Cors;
using AiCollect.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using AiCollect.Data.Providers;
using Microsoft.Extensions.Logging;

namespace AiCollect.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AiCollect_Cors_Policy")]
    public class UserController : BaseApiController
    {
        private ILogger<UserController> _logger;
        public UserController(ILogger<UserController> _logger)
        {
            this._logger = _logger;
        }

        [HttpGet]
        public User Get(int id)
        {
            try
            {
                return new UserProvider(DbInfo).GetUser(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("configuration/{Id}")]
        public Users GetConfigurationUsers(int Id)
        {
            try
            {
                return new UserProvider(DbInfo).ConfigurationUsers(Id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("configuration")]
        public bool AddConfigurationUser(User user)
        {
            try
            {
                return new UserProvider(DbInfo).AddConfigurationUser(user);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("admin")]
        public Users SuperAdmins()
        {
            try
            {
                return new UserProvider(DbInfo).GetSuperAdmins();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("client/{Id}")]
        public Users GetUserByClient(int Id)
        {
            try
            {
                return new UserProvider(DbInfo).ClientUsers(Id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("client")]
        public bool ClientUser(User user)
        {
            try
            {
                return new UserProvider(DbInfo).AddOrUpdateUser(user);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        public bool Post(User user)
        {
            try
            {
                return new UserProvider(DbInfo).AddOrUpdateUser(user);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("authentication")]
        public User Authentication(Credentials credentials)
        {
            try
            {
                return new UserProvider(DbInfo).AuthoriseUser(credentials);
            }
            catch (Exception ex)
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
                return new UserProvider(DbInfo).DeleteUser(id);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("configuration/delete")]
        public bool DeleteConfigurationUser(User user)
        {
            try
            {
                return new UserProvider(DbInfo).DeleteConfigurationUser(user);
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("verify")]
        public User VerifyUser(User _user)
        {
            try
            {
                return new UserProvider(DbInfo).VerifyUser(_user);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("password/reset")]
        public bool ResetPassword(User _user)
        {
            try
            {
                var isPasswordChanged = new UserProvider(DbInfo).ResetPassword(_user);
                if (isPasswordChanged)
                {
                    var innerHtml     = "   <h5> Hello AiCollect User,</h5>"
                                      + "   <p>" + "Your password has been succefully changed" + "</p>"
                                      + "   <h5 style='margin-top: 85px;'> Best Regards,</h5>"
                                      + "   <span> AICollect Team </span>";
                    Task.Run(() => MailService.SendMail(_user.Email, true, "AICollect Password Changed", innerHtml));
                }

                return isPasswordChanged;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("password/forgot")]
        public bool ForgotPassword(User _user)
        {
            try
            {
                User user = new UserProvider(DbInfo).GetUser(_user.Email);
                if (user != null)
                {
                    var innerHtml     = "   <h5> Hello AiCollect User,</h5>"
                                      + "   <p>" + "We have received a request to reset the password to your AICollect Account (" + user.Email + "). <br> Your verification code is : <strong>" + user.Usercode + "</strong> </p>"
                                      + "   <h5 style='margin-top: 85px;'> Best Regards,</h5>"
                                      + "   <span> AICollect Team </span>";
                    Task.Run(() => MailService.SendMail(_user.Email, true, "AICollect Verification Code", innerHtml));
                    return new UserProvider(DbInfo).AddOrUpdateUser(user);
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                throw ex;
            }
        }
    }
}
