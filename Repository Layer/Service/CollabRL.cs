using Common_Layer.Models;
using Repository_Layer.Context;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository_Layer.Service
{
    public class CollabRL : ICollabRL
    {
        public readonly FundooContext fundooContext; 

        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public bool AddCollab(CollabModel collabModel)
        {
            try
            {
                var noteData = fundooContext.NotesTable.Where(x => x.NoteId == collabModel.NoteId).FirstOrDefault();
                var userData = fundooContext.UserTable.Where(x => x.Email == collabModel.CollabEmail).FirstOrDefault();
                if (noteData != null && userData != null)
                {
                    CollabsEntity collab = new CollabsEntity();
                    collab.CollabsEmail = collabModel.CollabEmail;
                    collab.NoteId = collabModel.NoteId;
                    collab.Id = userData.Id;
                    //Adding the data to database
                    fundooContext.CollaboratorTable.Add(collab);
                }

                //Save the changes in database
                int result = fundooContext.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
        public string RemoveCollabs(long collabID, long userId)
        {
            var collab = fundooContext.CollaboratorTable.Where(X => X.CollabsID == collabID).FirstOrDefault();
            if (collab != null)
            {
                fundooContext.CollaboratorTable.Remove(collab);
                fundooContext.SaveChanges();
                return "Removed Successfully";
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<CollabsEntity> GetAllCollabs(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.CollaboratorTable.ToList().Where(x => x.NoteId == noteId);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
