using KIKDocumentAPIs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddScoped<IKIKService, KIKService>();
        services.AddHttpClient();
    })
    .Build();

host.Run();
