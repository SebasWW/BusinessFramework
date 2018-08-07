using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Tnomer.AspNetCore.Mvc.Logging
{
    public class ApplicationLog
    {
        public ILogger ControllerErrorLog { get; set; }
        public ILogger ControllerRequestLog { get; set; }

        static ApplicationLog _current;
        public static ApplicationLog Current
        {
            get
            {
                if (_current == null) _current = new ApplicationLog();
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        private static String _path = Path.Combine(Directory.GetCurrentDirectory(), "launchLog.txt");
        private static String _ok = "Ok." + Environment.NewLine;

        static public void LaunchLogNew(String message)
        {
            File.WriteAllText(_path, DateTime.Now.ToLongTimeString() + "   " + message);
        }
        static public void LaunchLogWrite(String message)
        {
            File.AppendAllText(_path, DateTime.Now.ToLongTimeString() + "   " + message);
        }
        static public void LaunchLogWriteOk()
        {
            File.AppendAllText(_path, _ok);
        }
    }
}
