using Microsoft.EntityFrameworkCore;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Context
{

    // A DbContext instance represents a session with the database and can be used to query and save the intances of  your entities.
    // DbContext is a combination of the Unit of Work and Repository patterns.
    public class FundooContext : DbContext // used for data accessibility
    {
            public FundooContext(DbContextOptions options)
                : base(options)
            {
            }
            public DbSet<UserEntity> UserTable { get; set; }
           public DbSet<NoteEntity> NotesTable { get; set; }
           public DbSet<CollabsEntity> CollaboratorTable { get; set; }
           public DbSet<LabelEntity> LabelsTable { get; set; }
    }
    
}