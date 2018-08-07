using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc.Configuration
{
    public class LoggingConfiguration
    {
        public LogConfiguration File { get; set; }
        //public LogConfiguration Launch { get; set; }
        public LogConfiguration ControllerError { get; set; }
        public LogConfiguration ControllerRequest { get; set; }
    }
}
