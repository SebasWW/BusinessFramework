using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework
{
    public abstract class BusinessException:Exception 
    {
        public String Key { get; private set; }

        internal BusinessException(String key, String message) : base(message)
        {
            Key = key;
        }
    }
}
