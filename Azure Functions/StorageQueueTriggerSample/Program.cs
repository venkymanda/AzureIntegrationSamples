using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PIM_MappingFunctions_Worker.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IPIMWorkerService, PIMWorkerService>();
        services.AddHttpClient();
    })
    .Build();

host.Run();
