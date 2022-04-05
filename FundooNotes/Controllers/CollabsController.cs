using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.Context;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabsController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        public CollabsController(ICollabBL collabBL, FundooContext fundooContext)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost("Add")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var collab = fundooContext.NotesTable.Where(X => X.NoteId == collabModel.NoteId).FirstOrDefault();
                if (collab.Id == userId)
                {
                    var result = collabBL.AddCollab(collabModel);
                    if (result)
                    {
                        return this.Ok(new { Success = true, message = "Collaboration stablish successfully", data = result });
                    }
                    else
                    {
                        return this.BadRequest(new { Sucess = false, message = "Collaboration stablish is Failed" });
                    }
                }
                else
                {
                    return this.BadRequest(new { Sucess = false, message = "Failed Collaboration" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

    }
}
