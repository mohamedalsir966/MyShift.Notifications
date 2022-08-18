using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyShift.Notifications;
using MyShift.Notifications.Service;
using Persistence.Repositories;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyShift.Notifications
{
  public  class Startup : FunctionsStartup
    {
       
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            builder.Services.AddScoped<IService, ServiceEng>();
            builder.Services.AddScoped<ILogsRepository, LogsRepository>();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=NotificationDB;Integrated Security=True"));

        }
    }
}
