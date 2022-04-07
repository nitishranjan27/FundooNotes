using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
    public class CollabsController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public CollabsController(ICollabBL collabBL, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
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
                    if (result != null)
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
                    return this.Unauthorized(new { Sucess = false, message = "Failed Collaboration" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
        [HttpDelete("Remove")]
        public IActionResult RemoveCollabs(long collabID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var delete = collabBL.RemoveCollabs(collabID, userId);
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Collaboration Removed Successfully", data = delete });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Collaboration  Unsuccessfully Removed" });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }
        [HttpGet("GetAll")]
        public IActionResult GetAllCollabs(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var notes = collabBL.GetAllCollabs(noteId,userId);
                if (notes != null)
                {
                    return this.Ok(new { Success = true, message = " All Collaborations Found Successfully", data = notes });

                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "No Collaborations  Found" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }

        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllCollaboratorUsingRedisCache()
        {
            var cacheKey = "CollabsList";
            string serializedList;
            var CollabsList = new List<CollabsEntity>();
            var redisCollabsList = await distributedCache.GetAsync(cacheKey);
            if (redisCollabsList != null)
            {
                serializedList = Encoding.UTF8.GetString(redisCollabsList);
                CollabsList = JsonConvert.DeserializeObject<List<CollabsEntity>>(serializedList);
            }
            else
            {
                CollabsList = await fundooContext.CollaboratorTable.ToListAsync();  // Comes from Microsoft.EntityFrameworkCore Namespace
                serializedList = JsonConvert.SerializeObject(CollabsList);
                redisCollabsList = Encoding.UTF8.GetBytes(serializedList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollabsList, options);
            }
            return Ok(CollabsList);
        }

    }
}