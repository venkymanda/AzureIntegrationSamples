using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
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
             * Protocol = BrokerProtocol.SaslSsl,
             * AuthenticationMode = BrokerAuthenticationMode.Plain,
             */
                  Username = "ConfluentCloudUserName",//API access key from the Confluent
                  Password = "ConfluentCloudPassword",//API secret obtained from the Confluent
                  Protocol = BrokerProtocol.SaslSsl,
                  AuthenticationMode = BrokerAuthenticationMode.Plain,

                  ConsumerGroup = "$Default")] string eventData, FunctionContext context)
        {
            var logger = context.GetLogger("KafkaFunction");
            logger.LogInformation($"C# Kafka trigger function processed a message: {JObject.Parse(eventData)["Value"]}");
            try
            {
                // Deserialize the Kafka message
                var kafkaMessage = JObject.Parse(eventData);

                // Extract the event type from the message
                if (kafkaMessage.TryGetValue("Headers", StringComparison.OrdinalIgnoreCase, out var eventTypeHeader))
                {
                    logger.LogInformation($"Received message with Event Type: {eventTypeHeader}");
                    var eventType = GetHeaderValue(eventTypeHeader, "EventType");
                    // Decode Base64-encoded event type
                    var decodedEventType = Encoding.UTF8.GetString(Convert.FromBase64String(eventType));
                    // Perform actions based on the event type
                    switch (decodedEventType.ToString())
                    {
                        case "Kafka-ServiceBusEvent":
                            logger.LogInformation($" message with Event Type is Handled Successfully: {decodedEventType}");
                            break;

                        default:
                            // Handle unknown event types
                            break;
                    }
                }
                else
                {
                    logger.LogWarning("Received message without an event type.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }

        private static string GetHeaderValue(JToken headers, string key)
        {
            if (headers is JToken headersArray)
            {
                var header = headersArray.FirstOrDefault(h => h["Key"]?.ToString() == key);
                if (header != null)
                {
                    return header["Value"]?.ToString();
                }
            }
            return null;
        }


    }
}
