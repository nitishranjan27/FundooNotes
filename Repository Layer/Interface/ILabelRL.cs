using Common_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity AddLabel(LabelModel labelModel);
        public IEnumerable<LabelEntity> GetAllLabels(long userId);
        public List<LabelEntity> Getlabel(long NotesId, long userId);
        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID);
    }
}
