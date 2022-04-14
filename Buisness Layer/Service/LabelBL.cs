using Buisness_Layer.Interface;
using Common_Layer.Models;
using Repository_Layer.Entity;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buisness_Layer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="labelRL">labelRL Parameter</param>
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }
        public LabelEntity AddLabel(LabelModel labelModel)
        {
            try
            {
                return labelRL.AddLabel(labelModel);
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
                return labelRL.GetAllLabels(userId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<LabelEntity> Getlabel(long NotesId, long userId)
        {
            try
            {
                return labelRL.Getlabel(NotesId,userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID)
        {
            try
            {
                return labelRL.UpdateLabel(labelModel,labelID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelEntity DeleteLabel(long labelID, long userId)
        {
            try
            {
                return labelRL.DeleteLabel(labelID,userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}