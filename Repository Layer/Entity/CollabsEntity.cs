using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Repository_Layer.Entity
{
    public class CollabsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabsID { get; set; }
        public string CollabsEmail { get; set; }

        [ForeignKey("User")]
        public long Id { get; set; }
        [JsonIgnore]
        public virtual UserEntity User { get; set; }

        [ForeignKey("Note")]
        public long NoteId { get; set; }
        [JsonIgnore]
        public virtual NoteEntity Note { get; set; }
    }
}
