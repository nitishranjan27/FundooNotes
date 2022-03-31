using Buisness_Layer.Interface;
using Common_Layer.Models;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL noteRL;
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
    }
}
