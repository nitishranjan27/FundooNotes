using System;
using System.Collections.Generic;
using System.Text;

namespace Common_Layer.Models
{
    public class NoteModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPinned { get; set; }
        public bool IsArchived { get; set; }
        public string Color { get; set; }
        public string BGImage { get; set; }
        public DateTime Reminder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        
    }
}
