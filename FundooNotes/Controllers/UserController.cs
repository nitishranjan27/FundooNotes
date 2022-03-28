using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}
