using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public class ConsistencyValidationException : BusinessException
    {
        internal ConsistencyValidationException(object id, string key, string message) : base(key, message)
        {
            Id = id;
        }
        internal ConsistencyValidationException(ConsistencyErrorEntry entry) : base(entry.Key, entry.Message)
        {
            Id = entry.Id;
        }

        public object Id { get; private set; }
    }
}
