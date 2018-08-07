using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc.Configuration.Google
{
    public class ReCaptureConfiguration
    {
        public Boolean Enabled { get; set; }
        public String SiteSecret { get; set; }
        public String ServiceUrl { get; set; }
    }
}
