using Buisness_Layer.Interface;
using Common_Layer.Models;
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
                    return BadRequest(new { success = false, message = "Data Not Successful Uploaded", data = res });
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { isSuccess = false, message = e.InnerException.Message });
            }
        }
        //[HttpPost("Login")]
        //public IActionResult LoginUser(UserLoginModel userLogin)
        //{
        //    try
        //    {
        //        var result = userBL.Login(userLogin);
        //        if (result != null)
        //        {
        //            return this.Ok(new { success = true, message = "Login Successfully", data = result });
        //        }
        //        else
        //            return this.BadRequest(new { success = false, message = "Login Unsuccessful" });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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
                    return this.BadRequest(new { Success = false, message = "Login Unsuccessful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.BadRequest(new { Success = false, Message = ex.Message });
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
                    _logger.LogError("forget successfull");
                    return this.BadRequest(new { Success = false, message = "Forget Password link UnSuccessfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, message = e.InnerException.Message });
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
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, message = e.InnerException.Message });
            }
        }
    }
}
