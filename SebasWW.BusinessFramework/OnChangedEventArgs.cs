using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public class OnChangedEventArgs<TObject> : EventArgs
        where TObject: BusinessObjectBase
    {
        public OnChangedEventArgs(TObject obj)
        {
            BusinessObject = obj;
        }

        public TObject BusinessObject { get; }
    }
}
