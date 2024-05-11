using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PIMMappingFunctions.Models;
using PIMMappingFunctions.ActivityFunctions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Grpc.Core;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Queues;
using Azure.Messaging;
using System.Security.Cryptography;

namespace PIMMappingFunctions.Orchestrator
{
    public static class PIMMappingFunctionsOrchestrator
    {


        /// <summary>
        /// The main orchestrator function that handles the file transfer process.
        /// </summary>
        /// <param name="context">The orchestration context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Function(nameof(MappingOrchestrator))]
        public static async Task<bool> MappingOrchestrator(
         [OrchestrationTrigger] TaskOrchestrationContext context)
        {

            ILogger logger = context.CreateReplaySafeLogger(nameof(MappingOrchestrator));
            logger.LogInformation("C# RunOrchestrator processed a request.");
            var reqbody = context.GetInput<string>();
            logger.LogInformation(reqbody);
            TaskInfo taskInfo = JsonConvert.DeserializeObject<TaskInfo>(reqbody);

            logger.LogInformation(taskInfo.callbackUrl);

            try
            {
                string functionname = "";
                if (taskInfo.requestbody.Contains("Purchase") && taskInfo.activity != "CombineCLAASPOUpload")
                {
                    functionname = "MapPurchaseOrderXMLforCLAAS";
                }

                var outputresponse = new JObject();
                var options = TaskOptions.FromRetryPolicy(new RetryPolicy(
                                                           maxNumberOfAttempts: 1,
                                             firstRetryInterval: TimeSpan.FromSeconds(5)));

                logger.LogInformation($"Activity Function Called {functionname}");
                using (var timeoutCts = new CancellationTokenSource())
                {
                    DateTime dueTime = context.CurrentUtcDateTime.AddSeconds(220);
                    Task<string> activityTask = context.CallActivityAsync<string>(functionname, taskInfo.requestbody, options);
                    Task durableTimeout = context.CreateTimer(dueTime, timeoutCts.Token);
                    var statusoftask = await Task.WhenAny(durableTimeout, activityTask);
                    //Task<bool> approvalEvent = context.WaitForExternalEvent<bool>("TransformationCompleted");
                    if (statusoftask == activityTask)
                    {
                        timeoutCts.Cancel();
                        taskInfo.xmlresponse = await activityTask;
                        taskInfo.isxml = true;
                        var nextResult = await context.CallActivityAsync<object>("PIMMappingCallback", JsonConvert.SerializeObject(taskInfo));
                    }
                    else
                    {
                        taskInfo.xmlresponse = "Activity Function didnt respond within 240 seconds, so cancelling the Orchestration";
                        taskInfo.isxml = false;
                        await context.CallActivityAsync<object>("PIMMappingCallback", JsonConvert.SerializeObject(taskInfo));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                taskInfo.iserror = true;
                taskInfo.jsonresponse = ex.Message;
                var nextResult = await context.CallActivityAsync<object>("PIMMappingCallback", JsonConvert.SerializeObject(taskInfo));
                return false;
            }
        }

    }
}