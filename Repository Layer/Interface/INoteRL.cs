using Common_Layer.Models;
using Microsoft.AspNetCore.Http;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface INoteRL
    {
        public NoteEntity CreateNote(NoteModel noteModel, long userId);
        public NoteEntity UpdateNote(NoteModel noteUpdateModel, long NoteId);
        public NoteEntity DeleteNotes(long NoteId);
        public IEnumerable<NoteEntity> GetAllNotes(long userId);
        public List<NoteEntity> GetNote(int NotesId);
        public NoteEntity ArchiveNote(long NoteId, long userId);
        public NoteEntity PinnedNote(long NoteId, long userId);
        public NoteEntity TrashedNote(long NotesId, long userId);
        public NoteEntity NoteColor(long NoteId, string addcolor);
        public NoteEntity UploadImage(IFormFile imageURL, long noteid);
        public NoteEntity DeleteImage(long noteid, long userId);
        
    }
}
