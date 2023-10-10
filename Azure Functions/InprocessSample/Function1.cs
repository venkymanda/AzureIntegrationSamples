using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Reflection.Metadata;
using System.Xml.Linq;
using InprocessSample.Services;

namespace InprocessSample
{
    public class HttpTriggerFunction
    {
        //Dependency Injection for Logger and Service Classes
        //If Logger needs to be used in your Service Classes you have to Include Logger as Ibjection and modify host.json file
        //You can refer to this article https://medium.com/medialesson/logging-in-azure-functions-f2b1a72a23eb which explains it very well
        private ISampleService _sampleService;
        private readonly ILogger<HttpTriggerFunction> _logger;
        public HttpTriggerFunction(ISampleService sampleService, ILogger<HttpTriggerFunction> logger)
        {
            _sampleService = sampleService;
            _logger = logger;
        }


    
            [FunctionName("HttpTriggerFunction")]
            public  async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                // Read the request content
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                _sampleService.HttpSendTest(requestBody);

                // Return a response
                return new OkObjectResult("Hello, Azure Functions!");
            }
        
    }
}
