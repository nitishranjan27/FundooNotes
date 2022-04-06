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
    }
}
