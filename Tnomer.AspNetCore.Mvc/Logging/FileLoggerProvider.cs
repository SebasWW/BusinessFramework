using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Tnomer.AspNetCore.Mvc.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _directory;
        public FileLoggerProvider(string directory)
        {
            _directory = directory;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(Path.Combine( _directory,DateTime.Now.ToString("yyyy-MM-dd_") +  categoryName + ".log"));
        }

        public void Dispose()
        {
        }
    }
}