﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSGlobal.Recorder
{
    class ShuttingDownMessage
    {
        public string CurrentViewName { get; private set; }
        public ShuttingDownMessage(string currentViewName) 
        {
            this.CurrentViewName = currentViewName;
        }
    }
}
