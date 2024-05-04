using System;
using System.Text;
using System.Text.Json.Serialization;
using Azure.Messaging;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PIM_MappingFunctions_Worker.Models;
using PIM_MappingFunctions_Worker.Services;


namespace PIM_MappingFunctions_Worker
{
    public class TransactionDemandWorker
    {
        private readonly ILogger<TransactionDemandWorker> _logger;
        private readonly IPIMWorkerService _pimworkerservice;

        public TransactionDemandWorker(ILogger<TransactionDemandWorker> logger,IPIMWorkerService pIMWorkerService)
        {
            _logger = logger;
            _pimworkerservice = pIMWorkerService;
        }

        [Function(nameof(TransactionDemandWorker))]
        [QueueOutput("%OutputQueue%")]
        public async Task<string> RunAsync([QueueTrigger("%InputQueue%", Connection = "storageconnectionstring")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
            // Generate a unique message ID
            string uniqueMessageId = message.MessageId;

            InputMessageQueue input = JsonConvert.DeserializeObject<InputMessageQueue>(message.MessageText);
            
            string result = input.WorkerName switch
            {
                string str when str=="SalesOrder" => await _pimworkerservice.SalesOrderConvertandUploadToBlob
                                                        (new List<string>()),
                _ => "Unsupported input type"
            };
            _logger.LogInformation($"File Transformed and Uploaded for : {uniqueMessageId}");
            // Create a new message with the unique message ID and the original message content
            var outputMessage = new OutputMessageQueue() { instanceId = uniqueMessageId ,Content=result};
            // Define the connection string and the name of the queue
            string connectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
            string queueName = Environment.GetEnvironmentVariable("OutputQueue");

           
            _logger.LogInformation($"Message with ID {uniqueMessageId} returned to output queue.");
            _logger.LogInformation($"File Uploaded Name : {result}");
            return JsonConvert.SerializeObject(outputMessage);
        }

       
    }
}
