using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public class WriteSecurityException : BusinessException
    {
        internal WriteSecurityException(object id, string key, string message) : base( message)
        {
            Id = id;
        }
        internal WriteSecurityException(SecurityErrorEntry entry) : base(entry.Message)
        {
            Id = entry.Id;
        }
        public object Id { get; }
    }
}
