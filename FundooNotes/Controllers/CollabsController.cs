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
    [Authorize] //user to grant and restrict permissions on Web pages.
    public class CollabsController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<UserController> _logger;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CollabsController"/> class.
        /// </summary>
        /// <param name="collabBL">collabBL parameter</param>
        /// <param name="fundooContext">fundooContext parameter</param>
        /// <param name="distributedCache">distributedCache parameter</param>
        /// <param name="logger">Logger</param>
        public CollabsController(ICollabBL collabBL, FundooContext fundooContext, IDistributedCache distributedCache, ILogger<UserController> logger)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this._logger = logger;
        }

        /// <summary>
        /// API for add collaborator
        /// </summary>
        /// <param name="collabModel">collabModel parameter</param>
        /// <returns>returns a new collaborator</returns>
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
                        _logger.LogInformation("Collaboration stablish successfully");
                        return this.Ok(new { Success = true, message = "Collaboration stablish successfully", data = result });
                    }
                    else
                    {
                        _logger.LogError("Collaboration stablish is Failed");
                        return this.BadRequest(new { Sucess = false, message = "Collaboration stablish is Failed" });
                    }
                }
                else
                {
                    _logger.LogError("Failed Collaboration");
                    return this.Unauthorized(new { Sucess = false, message = "Failed Collaboration" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        /// <summary>
        /// API for remove a collaborator from collaboration using collabID
        /// </summary>
        /// <param name="collabID">collabID parameter</param>
        /// <returns>remove member from collaboration</returns>
        [HttpDelete("Remove")]
        public IActionResult RemoveCollabs(long collabID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var delete = collabBL.RemoveCollabs(collabID, userId);
                if (delete != null)
                {
                    _logger.LogInformation("Collaboration Removed Successfully");
                    return this.Ok(new { Success = true, message = "Collaboration Removed Successfully", data = delete });
                }
                else
                {
                    _logger.LogError("Collaboration  Unsuccessfully Removed");
                    return this.BadRequest(new { Success = false, message = "Collaboration  Unsuccessfully Removed" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.Message, InnerException = e.InnerException });
            }
        }

        /// <summary>
        /// API for getting all collaborators
        /// </summary>
        /// <param name="noteId">noteId Parameter</param>
        /// <returns>returns all the collaborators</returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllCollabs(long noteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var notes = collabBL.GetAllCollabs(noteId,userId);
                if (notes != null)
                {
                    _logger.LogInformation(" All Collaborations Found Successfully");
                    return this.Ok(new { Success = true, message = " All Collaborations Found Successfully", data = notes });

                }
                else
                {
                    _logger.LogError("No Collaborations  Found");
                    return this.BadRequest(new { Success = false, message = "No Collaborations  Found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.BadRequest(new { Success = false, message = ex.InnerException.Message });
            }
        }

        /// <summary>
        /// API for get all collaborator using redis cache
        /// </summary>
        /// <returns>returns all the collaborators</returns>
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