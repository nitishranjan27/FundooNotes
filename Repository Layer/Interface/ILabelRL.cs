﻿using Common_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity AddLabel(LabelModel labelModel);
    }
}