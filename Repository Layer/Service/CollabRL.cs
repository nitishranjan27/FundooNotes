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
        public readonly FundooContext fundooContext;   //context class is used to query or save data to the database.

        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        /// <summary>
        /// Method for adding Members to collaboration
        /// </summary>
        /// <param name="collabModel">collabModel Parameter</param>
        /// <returns></returns>
        public CollabsEntity AddCollab(CollabModel collabModel)
        {
            try
            {
                var noteData = fundooContext.NotesTable.Where(x => x.NoteId == collabModel.NoteId).FirstOrDefault();
                var userData = fundooContext.UserTable.Where(x => x.Email == collabModel.CollabEmail).FirstOrDefault();
                if (noteData != null && userData != null)
                {
                    CollabsEntity collabsEntity = new CollabsEntity()
                    {
                        CollabsEmail = collabModel.CollabEmail,
                        NoteId = collabModel.NoteId,
                        Id = userData.Id
                    };
                    //Adding the data to database
                    fundooContext.CollaboratorTable.Add(collabsEntity);
                    //Save the changes in database
                    var result = fundooContext.SaveChanges();
                    return collabsEntity;
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

        /// <summary>
        /// Method for Remove member from collaboration
        /// </summary>
        /// <param name="collabID">collabID Parameter</param>
        /// <param name="userId">userId Parameter</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for getting all collaborator
        /// </summary>
        /// <param name="noteId">noteId Paramete</param>
        /// <param name="userId">userId Paramete</param>
        /// <returns></returns>
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
