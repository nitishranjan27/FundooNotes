using Buisness_Layer.Interface;
using Common_Layer.Models;
using FundooNotes.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserBL userBL, ILogger<UserController> logger)
        {
            this.userBL = userBL;
            this._logger = logger;
        }
        [HttpPost("Register")]
        public IActionResult Post(UserReg userReg)
        {
            try
            {
                var res = userBL.Registration(userReg);
                if(res != null)
                {
                    _logger.LogInformation("Register successfull");
                    return Ok(new { success = true, message = "Data Successful Uploaded" ,data=res});
                }
                else
                {
                    _logger.LogError("Register unsuccessfull");
                    throw new AppException("Data Not Successful Uploaded");
                }
            }
            catch(AppException e)
            {
                _logger.LogError(e.ToString());
                throw new AppException(e.Message);
            }
        }
        
        [HttpPost("Login")]
        public IActionResult UserLogin(UserLoginModel userLog)
        {
            try
            {
                var result = userBL.UserLogin(userLog);
                if (result != null)
                {
                    _logger.LogInformation("login successfull");
                    return this.Ok(new { Success = true, message = "Login Successfully", data = result });
                }
                else
                    _logger.LogError("login unsuccessfull");
                    throw new AppException("Login Unsuccessful");
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.ToString());
                throw new AppException(ex.Message);
            }
        }
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var result = userBL.ForgetPassword(email);
                if (result != null)
                {
                    _logger.LogInformation("forget successfull");
                    return this.Ok(new { Success = true, message = "Forget Password link Send Successfully" });
                }
                else
                    _logger.LogError("forget Unsuccessfull");
                    throw new AppException("Forget Password link UnSuccessfully");
            }
            catch (AppException e)
            {
                _logger.LogError(e.ToString());
                throw new AppException(e.Message);
            }
        }
        [Authorize]
        [HttpPost("ResetPassword")]

        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var result = userBL.ResetPassword(email, password, confirmPassword);
                _logger.LogInformation("reset successfull");
                return this.Ok(new { Success = true, message = "Password Reset Successfully" });

            }
            catch (AppException e)
            {
                _logger.LogError(e.ToString());
                throw new AppException(e.Message);
            }
        }
    }
}
