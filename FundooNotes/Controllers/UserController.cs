using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public IActionResult Post(UserReg userReg)
        {
            try
            {
                var res = userBL.Registration(userReg);
                if(res != null)
                {
                    return Ok(new { success = true, message = "Data Successful Uploaded" ,data=res});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Data Not Successful Uploaded", data = res });
                }
            }
            catch(Exception)
            {
                throw;
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
                    return this.Ok(new { success = true, message = "Login Successfully", data = result });
                }
                else
                    return this.BadRequest(new { success = false, message = "Login Unsuccessful" });
            }
            catch (Exception)
            {
                throw;
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
                    return this.Ok(new { isSuccess = true, message = "Forget Password link Send Successfully" });
                }
                else
                    return this.BadRequest(new { isSuccess = false, message = "Forget Password link UnSuccessfully" });
            }
            catch (Exception e)
            {

                return this.BadRequest(new { isSuccess = false, message = e.InnerException.Message });
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
                return this.Ok(new { isSuccess = true, message = "Password Reset Successfully" });

            }
            catch (Exception e)
            {

                return this.BadRequest(new { isSuccess = false, message = e.InnerException.Message });
            }
        }
    }
}
