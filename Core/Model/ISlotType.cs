﻿using Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model
{
    public interface ISlotType : IValidation
    {
        string GetSlotToElicit();
    }
}
