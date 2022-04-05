using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL notesBL;

        public NotesController(INoteBL notesBL)
        {
            this.notesBL = notesBL;
        }
        [HttpPost("Create")]
        public IActionResult CreateNote(NoteModel noteModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var note = notesBL.CreateNote(noteModel, userId);
                if (note != null)
                {
                    return this.Ok(new { Success = true, message = "Note Created Successfully ", data = note });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Note Create UnSuccessfull" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
        [HttpPut("Update")]
        public IActionResult UpdateNote(NoteModel noteUpdateModel, long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var result = notesBL.UpdateNote(noteUpdateModel, NoteId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
        
        [HttpDelete("Delete")]
        public IActionResult DeleteNotes(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var delete = notesBL.DeleteNotes(NoteId);
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes Deleted Successfully" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllNotes(long userId)
        {
            try
            {
                var notes = notesBL.GetAllNotes(userId);
                if (notes != null)
                {
                    return this.Ok(new { Success = true, message = "All notes found Successfully", data = notes });

                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
        [HttpGet("Get/{NotesId}")]
        public IActionResult GetNote(int NotesId)
        {
            try
            {
                long note = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                List<NoteEntity> notes = notesBL.GetNote(NotesId);
                if (notes != null)
                {
                    return this.Ok(new { Success = true, message = " Note Display Successfully", data = notes });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        [HttpPut("IsArchive")]
        public IActionResult ArchiveNote(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var archieve = notesBL.ArchiveNote(NoteId, userid);
                if (archieve != null)
                {
                    return this.Ok(new { Success = true, message = "Archived Successfully", data = archieve });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Archived UnSuccessfull" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        [HttpPut("IsPinned")]
        public IActionResult PinnedNote(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(p => p.Type == "Id").Value);
                var result = notesBL.PinnedNote(NoteId,userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Note Pinned Successfully" ,data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Note Pinned UnSuccessful" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }
        [HttpPut("IsTrash")]
        public IActionResult TrashedNote(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var result = notesBL.TrashedNote(NotesId,userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Trashed Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Trashed UnSuccessful" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }

        [HttpPut("Color")]
        public IActionResult NoteColor(long NoteId, string addcolor)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var color = notesBL.NoteColor(NoteId, addcolor);
                if (color != null)
                {
                    return this.Ok(new { Success = true, message = "Color Added Successfully", data = color });
                }
                else
                    return this.BadRequest(new { Success = false, message = " UnSuccessfully to Add Color" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }

        [HttpPut("UpdateImage/{noteid}")]
        public IActionResult UploadImage(long noteid,IFormFile imageURL)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var result = notesBL.UploadImage(imageURL, noteid);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Image is Added Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = " Sorry! Image is Not Added" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }


        [HttpDelete("DeleteImage/{noteid}")]
        public IActionResult DeleteImage(long noteid)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var delete = notesBL.DeleteImage(noteid, userId);
                if (noteid == 0)
                {
                    return this.BadRequest(new { Success = false, Message = "NoteID Not Excepted" });
                }
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Image Deleted Successfully" ,data = delete });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Image Not Deleted" });
                }
            }
            catch(Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }
    }
}
