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
    /// <summary>
    /// LabelsController connected with BaseController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // user to grant and restrict permissions on Web pages.
    public class LabelsController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelsController"/> class.
        /// </summary>
        /// <param name="labelBL">labelBL parameter</param>
        /// <param name="fundooContext">fundooContext parameter</param>
        /// <param name="distributedCache">distributedCache parameter</param>
        /// <param name="logger">Logger</param>
        public LabelsController(ILabelBL labelBL, FundooContext fundooContext, IDistributedCache distributedCache, ILogger<UserController> logger)
        {
            this.labelBL = labelBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this._logger = logger;
        }

        /// <summary>
        /// API for adding a new label
        /// </summary>
        /// <param name="labelModel">labelModel parameter</param>
        /// <returns>returns a new label</returns>
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
                        _logger.LogInformation("Label created successfully");
                        return this.Ok(new { Success = true, Message = "Label created successfully", data = result });
                    }
                    else
                    {
                        _logger.LogError("Label not created");
                        return this.BadRequest(new { Success = false, Message = "Label not created" });
                    }
                }
                else
                {
                    _logger.LogError("Unauthorized User!");
                    return this.Unauthorized(new { Success = false, Message = "Unauthorized User!" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API for getting all labels
        /// </summary>
        /// <param name="userId">userId Parameter</param>
        /// <returns>returns all labels</returns>
        [HttpGet("GetAll")]
        public IActionResult GetAllLabels()
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var labels = labelBL.GetAllLabels(userid);
                if (labels != null)
                {
                    _logger.LogInformation(" All labels Showing Successfully");
                    return this.Ok(new { Success = true, Message = " All labels found Successfully", data = labels });
                }
                else
                {
                    _logger.LogError("No label found");
                    return this.NotFound(new { Success = false, Message = "No label found" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API for Getting all notes using redis cache
        /// </summary>
        /// <returns>returns all notes</returns>
        [HttpGet("Redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "LabelsList";
            string serializedLabelsList;
            var labelsList = new List<LabelEntity>();
            var redisLabelsList = await this.distributedCache.GetAsync(cacheKey);
            if (redisLabelsList != null)
            {
                serializedLabelsList = Encoding.UTF8.GetString(redisLabelsList);
                labelsList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelsList);
            }
            else
            {
                labelsList = await this.fundooContext.LabelsTable.ToListAsync();  // Comes from Microsoft.EntityFrameworkCore Namespace
                serializedLabelsList = JsonConvert.SerializeObject(labelsList);
                redisLabelsList = Encoding.UTF8.GetBytes(serializedLabelsList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await this.distributedCache.SetAsync(cacheKey, redisLabelsList, options);
            }

            return this.Ok(labelsList); 
        }

        /// <summary>
        /// API for Get Labels by noteId
        /// </summary>
        /// <param name="NotesId">NotesId parameter</param>
        /// <returns>returns all labels using notesId</returns>
        [HttpGet("Get/{NotesId}")]
        public IActionResult Getlabel(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var labels = labelBL.Getlabel(NotesId,userId);
                if (labels != null)
                {
                    _logger.LogInformation(" Specific label found Successfully");
                    return this.Ok(new { Success = true, message = " Specific label found Successfully", data = labels });
                }
                else
                {
                    _logger.LogError("No label found");
                    return this.NotFound(new { Success = false, message = "Specific label not Found" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API for update a label by labelId
        /// </summary>
        /// <param name="labelModel">labelModel parameter</param>
        /// <param name="labelID">labelID parameter</param>
        /// <returns>returns for updated labels</returns>
        [HttpPut("Update")]
        public IActionResult UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var result = labelBL.UpdateLabel(labelModel, labelID);
                if (result != null)
                {
                    _logger.LogInformation("Label Updated Successfully");
                    return this.Ok(new { Success = true, message = "Label Updated Successfully", data = result });
                }
                else
                {
                    _logger.LogError("Label Not Updated");
                    return this.NotFound(new { Success = false, message = "Label Not Updated" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// API for delete a label by labelID
        /// </summary>
        /// <param name="labelID">labelID parameter</param>
        /// <returns>Delete a label</returns>
        [HttpDelete("Delete")]
        public IActionResult DeleteLabel(long labelID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "Id").Value);
                var delete = labelBL.DeleteLabel(labelID, userId);
                if (delete != null)
                {
                    _logger.LogInformation("Label Deleted Successfully");
                    return this.Ok(new { Success = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    _logger.LogError("Label Not Deleted");
                    return this.NotFound(new { Success = false, message = "Label not Deleted" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return this.BadRequest(new { Success = false, Message = e.InnerException.Message });
            }
        }
    }
}