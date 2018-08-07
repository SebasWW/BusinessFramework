using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tnomer.AspNetCore.Mvc.Configuration
{
    public class ApplicationConfiguration
    {
        private static object objLock = new object();

        public GoogleConfiguration Google { get; set; }
        public MicrosoftConfiguration Microsoft { get; set; }

        public CorsConfiguration Cors { get; set;}
        public CdnConfiguration Cdn { get; set; }
        public RoutingConfiguration Routing { get; set; }
        public LoggingConfiguration Logging { get; set; }
        public JwtConfiguration Jwt { get; set; }

        static ApplicationConfiguration _current;
        public static ApplicationConfiguration Current
        {
            get
            {
                if (_current == null) _current = new ApplicationConfiguration();
                return _current;
            }
            set
            {
                _current = value;
            }
        }
    }
}
