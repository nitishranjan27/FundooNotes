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
    public class LabelRL : ILabelRL
    {
        public readonly FundooContext fundooContext;
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public LabelEntity AddLabel(LabelModel labelModel)
        {
            try
            {
                // checking with the notestable db to find NoteId
                var note = fundooContext.NotesTable.Where(x => x.NoteId == labelModel.NoteId).FirstOrDefault();
                if (note != null)
                {
                    // Entity class Instance
                    LabelEntity label = new LabelEntity();
                    label.LabelName = labelModel.LabelName;
                    label.NoteId = note.NoteId;
                    label.Id = note.Id;

                    fundooContext.LabelsTable.Add(label);
                    int result = fundooContext.SaveChanges();
                    return label;
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

        public IEnumerable<LabelEntity> GetAllLabels(long userId)
        {
            try
            {
                var result = fundooContext.LabelsTable.ToList().Where(x => x.Id == userId);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<LabelEntity> Getlabel(long NotesId, long userId)
        {
            try
            {
                var response = fundooContext.LabelsTable.Where(x => x.NoteId == NotesId).ToList();
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                var update = fundooContext.LabelsTable.Where(X => X.LabelID == labelID).FirstOrDefault();
                if (update != null && update.LabelID == labelID)
                {
                    update.LabelName = labelModel.LabelName;
                    update.NoteId = labelModel.NoteId;

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
