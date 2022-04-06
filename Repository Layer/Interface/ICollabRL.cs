using Common_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface ICollabRL
    {
        public CollabsEntity AddCollab(CollabModel collabModel);
        public string RemoveCollabs(long collabID, long userId);
        public IEnumerable<CollabsEntity> GetAllCollabs(long noteId, long userId);
    }
}
