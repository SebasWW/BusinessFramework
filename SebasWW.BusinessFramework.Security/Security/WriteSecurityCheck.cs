using System;
using System.Collections.Generic;
using System.Linq;

namespace SebasWW.BusinessFramework.Security
{
    public class WriteSecurityCheck
    {
        public WriteSecurityState State = WriteSecurityState.NoAccess;
        public IQueryable<SecurityErrorEntry> Queue;
    }
}
