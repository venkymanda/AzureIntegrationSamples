using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PIMMappingFunctions.Models;
using PIMMappingFunctions.ActivityFunctions;
using PIMMappingFunctions.Orchestrator;

using PIMMappingFunctions.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;

namespace PIMMappingFunctions.Trigger
{
    public  class PIMPurchaseOrderMappingFunctionsTrigger : BaseHttpFunction
    {

        [Function("PIMPurchaseOrderMappingFunctionsTrigger")]
        public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("PIMPurchaseOrderMappingFunctionsTrigger");


            string callbackUrl = req.Query["callbackUrl"];
            string activity = req.Query["activity"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
          

            TaskInfo taskInfo = JsonConvert.DeserializeObject<TaskInfo>(requestBody);
            

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(PIMMappingFunctionsOrchestrator.MappingOrchestrator), requestBody);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);


            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return client.CreateCheckStatusResponse(req, instanceId);
  
         
        }

       
    }
}