using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.Context;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly FundooContext fundooContext;

        public LabelsController(ILabelBL labelBL, FundooContext fundooContext)
        {
            this.labelBL = labelBL;
            this.fundooContext = fundooContext;
        }

        [HttpPost("Create")]
        public IActionResult AddLabel(LabelModel labelModel) 
        {
            try
            {
                //checking if the user has a claim to access.
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var labelNote = fundooContext.NotesTable.Where(x => x.NoteId == labelModel.NoteId).FirstOrDefault();
                if (labelNote.Id == userid)
                {
                    var result = labelBL.AddLabel(labelModel);
                    if (result != null)
                    {
                        return this.Ok(new { Success = true, Message = "Label created successfully", data = result });
                    }
                    else
                    {
                        return this.BadRequest(new { Success = false, Message = "Label not created" });
                    }
                }
                return this.Unauthorized(new { Success = false, Message = "Unauthorized User!" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }
    }
}
