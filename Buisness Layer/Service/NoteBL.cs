using Buisness_Layer.Interface;
using Common_Layer.Models;
using Microsoft.AspNetCore.Http;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Service
{
    public class NoteBL : INoteBL
    {

        /// <summary>
        /// Variable
        /// </summary>
        private readonly INoteRL noteRL;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteRL">noteRL Parameter</param>
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public NoteEntity ArchiveNote(long NoteId,long userId)
        {
            try
            {
                return noteRL.ArchiveNote(NoteId,userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adding a new Note Method
        /// </summary>
        /// <param name="noteModel">noteModel Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
        public NoteEntity CreateNote(NoteModel noteModel, long userId)
        {
            try
            {
                return noteRL.CreateNote(noteModel, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity DeleteImage(long noteid, long userId)
        {
            try
            {
                return noteRL.DeleteImage(noteid,userId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public NoteEntity DeleteNotes(long NoteId)
        {
            try
            {
                return noteRL.DeleteNotes(NoteId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetAllNotes(long userId)
        {
            try
            {
                return noteRL.GetAllNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NoteEntity> GetNote(int NotesId)
        {
            try
            {
                return noteRL.GetNote(NotesId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity NoteColor(long NoteId, string addcolor)
        {
            try
            {
                return noteRL.NoteColor(NoteId,addcolor);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public NoteEntity PinnedNote(long NoteId, long userId)
        {
            try
            {
                return noteRL.PinnedNote(NoteId,userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NoteEntity TrashedNote(long NotesId, long userId)
        {
            try
            {
                return noteRL.TrashedNote(NotesId,userId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public NoteEntity UpdateNote(NoteModel noteUpdateModel, long NoteId)
        {
            try
            {
                return noteRL.UpdateNote(noteUpdateModel, NoteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity UploadImage(IFormFile imageURL, long noteid)
        {
            try
            {
                return noteRL.UploadImage(imageURL, noteid);
            }
            catch(Exception) 
            {
                throw;
            }
        }
    }
}
