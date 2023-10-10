using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using InprocessSample.Services;
//using Seges_Functions_POC.Services;
//using ServiceReference1;

[assembly: FunctionsStartup(typeof(InprocessSample.Startup))]
namespace InprocessSample
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
           
            builder.Services.AddHttpClient();
            builder.Services.AddTransient<ISampleService, SampleService>();
            //builder.Services.AddLogging();

        }
    }
}
