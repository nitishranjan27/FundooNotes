using Common_Layer.Models;
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
        public readonly FundooContext fundooContext;
        public NoteRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

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

        public NoteEntity CreateNote(NoteModel noteModel,long userId)
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
            catch(Exception) 
            {
                throw;
            }
        }

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
            catch(Exception)
            {
                throw;
            }
        }

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
    }    
}
