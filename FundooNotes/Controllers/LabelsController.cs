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

        [HttpGet("GetAll")]
        public IActionResult GetAllLabels(long userId)
        {
            try
            {
                var labels = labelBL.GetAllLabels(userId);
                if (labels != null)
                {
                    return this.Ok(new { Success = true, Message = " All labels found Successfully", data = labels });
                }
                else
                {
                    return this.NotFound(new { Success = false, Message = "No label found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        [HttpGet("Get/{NotesId}")]
        public IActionResult Getlabel(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var labels = labelBL.Getlabel(NotesId,userId);
                if (labels != null)
                {
                    return this.Ok(new { Success = true, message = " Specific label found Successfully", data = labels });
                }
                else
                    return this.NotFound(new { Success = false, message = "Specific label not Found" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        [HttpPut("Update")]
        public IActionResult UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var result = labelBL.UpdateLabel(labelModel, labelID);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Label Updated Successfully", data = result });
                }
                else
                {
                    return this.NotFound(new { Success = false, message = "Label Not Updated" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

    }
}
