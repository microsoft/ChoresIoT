using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ChoreIot;
using Microsoft.AspNetCore.Http;
[assembly: FunctionsStartup(typeof(ChoreIoT.Startup))]

namespace ChoreIoT
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ChoreService, ChoreService>();
        }
    }
}