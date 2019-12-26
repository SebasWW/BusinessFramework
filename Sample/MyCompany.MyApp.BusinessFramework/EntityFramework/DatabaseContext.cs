using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.EntityFramework;
using MyCompany.MyApp.Config;
using System.Linq;

namespace MyCompany.MyApp.EntityFramework
{
    public class DatabaseContext : MyAppDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(AppConfig.GetAppConfig().ConnectionString)
                .EnableSensitiveDataLogging();
        }

        internal DatabaseContext() { }
    }
}
