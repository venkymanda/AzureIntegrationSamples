using System.Net;
using KIKDocumentAPIs.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp2
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IKIKService _kikservice;

        public Function1(ILoggerFactory loggerFactory,IKIKService kIKService)
        {
            _logger = loggerFactory.CreateLogger<KIKService>();
            _kikservice = kIKService;

        }

        [Function("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            _kikservice.HttpSendTest("te");
            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
