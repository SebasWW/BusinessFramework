﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public class OnChangingEventArgs<TObject> : EventArgs
        where TObject : BusinessObjectBase
    {
        public OnChangingEventArgs(TObject obj)
        {
            BusinessObject = obj;
        }

        public TObject BusinessObject { get; }
    }
}