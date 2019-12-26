using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MyCompany.MyApp.Config
{
    public class AppConfig
    {
        public String ConnectionString { get; set; }

        static object objLock = new object();
        static AppConfig _appConfig;
        internal static AppConfig GetAppConfig()
        {
            if(_appConfig == null)
                lock (objLock)
                    {
                    // Set up configuration sources.
                    IConfigurationBuilder builder = new ConfigurationBuilder();
                            //.SetBasePath(Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location))
                            //.AddJsonFile(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".json", optional: true, reloadOnChange: true);

                        _appConfig = builder.Build().GetSection("App").Get<AppConfig>();
                }

            return _appConfig;
        }
    }
}
