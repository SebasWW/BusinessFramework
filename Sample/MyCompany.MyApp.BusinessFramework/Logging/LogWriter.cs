using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyCompany.MyApp.EntityFramework;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Log;

namespace MyCompany.MyApp.Log 
{
    internal class LogWriter : ILogWriter
    {
        public async Task Save(BusinessManager context, IEnumerable<LogEntry> logs)
        {
            foreach( var l in logs)
            {
                var log = new SLog();

                log.HostName = "unknown" ;
                log.CreateDate = DateTime.Now;

                log.TableId = (await Cache.MyAppCacheManager.GetTablesAsync()).Where(t => t.Key == l.DbSetName).FirstOrDefault().Value;
                log.RowId = (Int32)l.PrimaryKey;
                log.ColumnName = l.PropertyName;
                log.Value = l.NewValue?.ToString();
                log.UserId = ((MyAppManager)context).User.Id;
				log.SArchive = 0;

                ((MyAppManager) context).DbContext.Set<SLog>().Add(log);
            }

           await (context as MyAppManager).DbContext.SaveChangesAsync();
        }
    }
}
