using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace KafkaListenerSample
{
    public class KafkaListenerLocalCluster
    {
        private readonly ILogger _logger;

        public KafkaListenerLocalCluster(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<KafkaListenerLocalCluster>();
        }

        [Function("KafkaTrigger")]
        public static void Run(
            [KafkaTrigger("BrokerList",
                  "topic",
            /*
             * when using Clnfluent cloud cluster use below attributes as well
             * 
             * Username = "ConfluentCloudUserName",//API access key from the Confluent
             * Password = "ConfluentCloudPassword",//API secret obtained from the Confluent
             */
                  ConsumerGroup = "$Default")] string eventData, FunctionContext context)
        {
            var logger = context.GetLogger("KafkaFunction");
            logger.LogInformation($"C# Kafka trigger function processed a message: {JObject.Parse(eventData)["Value"]}");
        }
    }
}
