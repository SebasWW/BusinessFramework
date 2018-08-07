using System;
using System.Collections.Generic;
using System.Text;

namespace Tnomer.Net.Http
{
   public class UrlParameter
    {
        public UrlParameter(String name, String value)
        {
            Name = name;
            Value = value;
        }

        public String Name { get; set; }
        public String Value { get; set; }
    }
    
}
