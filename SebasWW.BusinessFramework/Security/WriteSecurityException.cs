using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public class WriteSecurityException : BusinessException
    {
        internal WriteSecurityException(Object id, String key, String message) : base(key, message)
        {
            Id = id;
        }
        internal WriteSecurityException(SecurityErrorEntry entry) : base(entry.Key, entry.Message)
        {
            Id = entry.Id;
        }
        public Object Id { get; private set; }
    }
}
