using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public interface IRemovable
    {
        //void Remove();
        Boolean IsDeleted { get; }
    }
}
