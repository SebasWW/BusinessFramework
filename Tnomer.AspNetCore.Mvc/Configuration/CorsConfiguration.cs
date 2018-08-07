using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Tnomer.AspNetCore.Mvc.Configuration
{
    public class CorsConfiguration
    {
        public Boolean UseCors { get; set; }
        public Boolean AllowAnyOrigin { get; set; }
        public Boolean AllowAnyMethod { get; set; }
        public Boolean AllowAnyHeader { get; set; }
        public Boolean AllowCredentials { get; set; }
        public String WithOrigin { get; set; }
        public String WithMethod { get; set; }
        public String WithHeader { get; set; }

        public void Build(IApplicationBuilder app)
        {
            if (UseCors)
            {
                app.UseCors(
                    t => 
                        { 
                            //t.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                            if (AllowAnyHeader) { t = t.AllowAnyHeader(); } else if (WithHeader.Length > 0) t = t.WithHeaders(WithHeader);
                            if (AllowAnyMethod) { t = t.AllowAnyMethod(); } else if (WithHeader.Length > 0) t = t.WithHeaders(WithMethod);
                            if (AllowAnyOrigin) { t = t.AllowAnyOrigin(); } else if (WithHeader.Length > 0) t = t.WithHeaders(WithOrigin);
                            if (AllowCredentials) { t = t.AllowCredentials(); } else t = t.DisallowCredentials();
                        }
                    );
            }
        }
    }
}
