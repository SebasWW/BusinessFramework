using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SebasWW.BusinessFramework.Log
{
    public interface ILogWriter
    {
        Task SaveAsync(BusinessContext context, IEnumerable<LogEntry> logs);
    }
}
