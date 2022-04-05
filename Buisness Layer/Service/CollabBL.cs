using Buisness_Layer.Interface;
using Common_Layer.Models;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Service
{
    public class CollabBL : ICollabBL
    {
        private readonly ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }
        public bool AddCollab(CollabModel collabModel)
        {
            try
            {
                return collabRL.AddCollab(collabModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<CollabsEntity> GetAllCollabs(long noteId, long userId)
        {
            try
            {
                return collabRL.GetAllCollabs(noteId,userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string RemoveCollabs(long collabID, long userId)
        {
            try
            {
                return collabRL.RemoveCollabs(collabID,userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
