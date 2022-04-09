using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteBL notesBL;
        private readonly FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<UserController> _logger;

        public NotesController(INoteBL notesBL, FundooContext fundooContext, IDistributedCache distributedCache, ILogger<UserController> logger)
        {
            this.notesBL = notesBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this._logger = logger;
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
                    _logger.LogInformation("Note Created Successfully ");
                    return this.Ok(new { Success = true, message = "Note Created Successfully ", data = note });
                }
                else
                {
                    _logger.LogError("Note Not Created");
                    return this.BadRequest(new { Success = false, message = "Note Create UnSuccessfull" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
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
                    _logger.LogInformation("Notes Updated Successfully ");
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    _logger.LogError("Note Not Updated");
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
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
                    _logger.LogInformation("Notes Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    _logger.LogError("Note Not Deleted");
                    return this.BadRequest(new { Success = false, message = "Notes Deleted Successfully" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
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
                    _logger.LogInformation("All notes Showing Successfully");
                    return this.Ok(new { Success = true, message = "All notes found Successfully", data = notes });

                }
                else
                {
                    _logger.LogError("Note Not Found");
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string serializedNotesList;
            var notesList = new List<NoteEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNotesList);
            }
            else
            {
                notesList = await fundooContext.NotesTable.ToListAsync();  // Comes from Microsoft.EntityFrameworkCore Namespace
                serializedNotesList = JsonConvert.SerializeObject(notesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }

            return this.Ok(notesList);
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
                    _logger.LogInformation(" Note Display Successfully");
                    return this.Ok(new { Success = true, message = " Note Display Successfully", data = notes });
                }
                else
                {
                    _logger.LogError("Note Not Found");
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
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
                    _logger.LogInformation("Archived Successfully");
                    return this.Ok(new { Success = true, message = "Archived Successfully", data = archieve });
                }
                else
                {
                    _logger.LogError("Archived UnSuccessfull");
                    return this.BadRequest(new { Success = false, Message = "Archived UnSuccessfull" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
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
                    _logger.LogInformation("Note Pinned Successfully");
                    return this.Ok(new { Success = true, message = "Note Pinned Successfully" ,data = result });
                }
                else
                {
                    _logger.LogError("Note Pinned UnSuccessful");
                    return this.BadRequest(new { Success = false, message = "Note Pinned UnSuccessful" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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
                    _logger.LogInformation("Trashed Successfully");
                    return this.Ok(new { Success = true, message = "Trashed Successfully", data = result });
                }
                else
                {
                    _logger.LogError("Trashed UnSuccessful");
                    return this.BadRequest(new { Success = false, message = "Trashed UnSuccessful" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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
                    _logger.LogInformation("Color Added Successfully");
                    return this.Ok(new { Success = true, message = "Color Added Successfully", data = color });
                }
                else
                {
                    _logger.LogError(" UnSuccessfully to Add Color");
                    return this.BadRequest(new { Success = false, message = " UnSuccessfully to Add Color" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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
                    _logger.LogInformation("Image Uploded Successfully");
                    return this.Ok(new { Success = true, message = "Image is Added Successfully", data = result });
                }
                else
                {
                    _logger.LogError("Image Not Added");
                    return this.BadRequest(new { Success = false, message = " Sorry! Image is Not Added" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
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
                    _logger.LogError("Zero Request Not Accepted");
                    return this.BadRequest(new { Success = false, Message = "NoteID Not Excepted" });
                }
                if (delete != null)
                {
                    _logger.LogInformation("Image Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Image Deleted Successfully" ,data = delete });
                }
                else
                {
                    _logger.LogError("Image Not Deleted");
                    return this.BadRequest(new { Success = false, message = "Image Not Deleted" });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }
    }
}
