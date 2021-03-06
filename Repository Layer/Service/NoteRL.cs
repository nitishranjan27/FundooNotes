using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository_Layer.Service
{
    public class NoteRL : INoteRL
    {
        /// <summary>
        /// Defining Variables
        /// </summary>
        public readonly FundooContext fundooContext; //context class is used to query or save data to the database.
        public readonly IConfiguration appsettings;  //IConfiguration interface is used to read Settings and Connection Strings from AppSettings.
        public NoteRL(FundooContext fundooContext, IConfiguration Appsettings)
        {
            this.fundooContext = fundooContext;
            this.appsettings = Appsettings;
        }

        /// <summary>
        /// Adding Archive Method
        /// </summary>
        /// <param name="NoteId">NoteId Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        public NoteEntity ArchiveNote(long NoteId, long userId)
        {
            try
            {
                var response = fundooContext.NotesTable.Where(A => A.NoteId == NoteId && A.Id == userId).FirstOrDefault();
                if (response != null)
                {
                    if (response.IsArchived == false)
                    {
                        response.IsArchived = true;
                    }
                    else
                    {
                        response.IsArchived = false;
                    }
                    fundooContext.SaveChanges();
                    return response;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creating a CreateNote Method
        /// </summary>
        /// <param name="noteModel">noteModel Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public NoteEntity CreateNote(NoteModel noteModel, long userId)
        {
            try
            {
                NoteEntity newNotes = new NoteEntity();

                newNotes.Id = userId;
                newNotes.Title = noteModel.Title;
                newNotes.Body = noteModel.Body;
                newNotes.Reminder = noteModel.Reminder;
                newNotes.Color = noteModel.Color;
                newNotes.BGImage = noteModel.BGImage;
                newNotes.IsArchived = noteModel.IsArchived;
                newNotes.IsPinned = noteModel.IsPinned;
                newNotes.IsDeleted = noteModel.IsDeleted;
                newNotes.CreatedAt = DateTime.Now;

                //Adding the data to database
                fundooContext.NotesTable.Add(newNotes);
                //Save the changes in database
                int result = fundooContext.SaveChanges();
                if (result > 0)
                {
                    return newNotes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete Method for deleting NoteId
        /// </summary>
        /// <param name="NoteId">NoteId Parameter</param>
        /// <returns></returns>
        public NoteEntity DeleteNotes(long NoteId)
        {
            try
            {
                var deleteNote = fundooContext.NotesTable.Where(X => X.NoteId == NoteId).FirstOrDefault();
                if (deleteNote != null)
                {
                    fundooContext.NotesTable.Remove(deleteNote);
                    fundooContext.SaveChanges();
                    return deleteNote;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Show all notes of all users
        /// </summary>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<NoteEntity> GetAllNotes(long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.ToList().Where(x => x.Id == userId);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Show note for particular user
        /// </summary>
        /// <param name="NotesId">NotesId Parameter</param>
        /// <returns></returns>
        public List<NoteEntity> GetNote(int NotesId)
        {
            try
            {
                var listNote = fundooContext.NotesTable.Where(X => X.NoteId == NotesId).FirstOrDefault();
                if (listNote != null)
                {
                    return fundooContext.NotesTable.Where(list => list.NoteId == NotesId).ToList();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adding Note Color
        /// </summary>
        /// <param name="NoteId">NotesId Parameter</param>
        /// <param name="addcolor">addcolor Parameter</param>
        /// <returns></returns>
        public NoteEntity NoteColor(long NoteId, string addcolor)
        {
            var note = fundooContext.NotesTable.Where(c => c.NoteId == NoteId).FirstOrDefault();
            if (note != null)
            {
                if (addcolor != null)
                {
                    note.Color = addcolor;
                    fundooContext.NotesTable.Update(note);
                    fundooContext.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adding pin for Note
        /// </summary>
        /// <param name="NoteId">NotesId Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        public NoteEntity PinnedNote(long NoteId, long userId)
        {
            var pin = fundooContext.NotesTable.Where(p => p.NoteId == NoteId && p.Id == userId).FirstOrDefault();
            if (pin != null)
            {
                if (pin.IsPinned == false)
                {
                    pin.IsPinned = true;
                }
                else
                {
                    pin.IsPinned = false;
                }
                fundooContext.SaveChanges();
                return pin;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adding Trash for Note
        /// </summary>
        /// <param name="NotesId">NotesId Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        public NoteEntity TrashedNote(long NotesId, long userId)
        {
            var trashed = fundooContext.NotesTable.Where(t => t.NoteId == NotesId && t.Id == userId).FirstOrDefault();
            if (trashed != null)
            {
                if (trashed.IsDeleted == false)
                {
                    trashed.IsDeleted = true;
                }
                else
                {
                    trashed.IsDeleted = false;
                }
                fundooContext.SaveChanges();
                return trashed;

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Updating Note
        /// </summary>
        /// <param name="noteUpdateModel">noteUpdateModel Parameter</param>
        /// <param name="NoteId">NotesId Parameter</param>
        /// <returns></returns>
        public NoteEntity UpdateNote(NoteModel noteUpdateModel, long NoteId)
        {
            try
            {
                var update = fundooContext.NotesTable.Where(X => X.NoteId == NoteId).FirstOrDefault();
                if (update != null)
                {
                    update.Title = noteUpdateModel.Title;
                    update.Body = noteUpdateModel.Body;
                    update.ModifiedAt = DateTime.Now;
                    update.Color = noteUpdateModel.Color;
                    update.BGImage = noteUpdateModel.BGImage;
                    fundooContext.NotesTable.Update(update);
                    fundooContext.SaveChanges();
                    return update;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Add Image in Note
        /// </summary>
        /// <param name="imageURL">imageURL Parameter</param>
        /// <param name="noteid">noteid Parameter</param>
        /// <returns></returns>
        public NoteEntity UploadImage(IFormFile imageURL, long noteid)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = fundooContext.NotesTable.Where(i => i.NoteId == noteid).FirstOrDefault();
                    if (note != null)
                    {
                        Account acc = new Account(
                            appsettings["Cloudinary:cloud_name"],
                            appsettings["Cloudinary:api_key"],
                            appsettings["Cloudinary:api_secret"]
                            );
                        Cloudinary Cld = new Cloudinary(acc);
                        var path = imageURL.OpenReadStream();
                        ImageUploadParams upLoadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(imageURL.FileName, path)
                        };
                        var UploadResult = Cld.Upload(upLoadParams);
                        note.BGImage = UploadResult.Url.ToString();
                        note.ModifiedAt = DateTime.Now;
                        fundooContext.SaveChanges();
                        return note;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Deleting the Image
        /// </summary>
        /// <param name="noteid">noteid Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        public NoteEntity DeleteImage(long noteid, long userId)
        {
            try
            {
                if (noteid > 0)
                {
                    var note = fundooContext.NotesTable.Where(x => x.NoteId == noteid).FirstOrDefault();
                    if (note != null)
                    {
                        note.BGImage = "";
                        note.ModifiedAt = DateTime.Now;
                        fundooContext.SaveChanges();
                        return note;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
