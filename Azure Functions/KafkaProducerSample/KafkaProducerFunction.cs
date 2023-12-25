
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

public static class KafkaProducerFunction
{
    private static readonly string KafkaBroker = Environment.GetEnvironmentVariable("KAFKA_BROKER")??"";
    private static readonly string KafkaTopic = Environment.GetEnvironmentVariable("KAFKA_TOPIC")??"";

    [Function("KafkaProducerFunction")]
    public static void Run(
        [TimerTrigger("*/5 * * * * *")] TimerInfo myTimer,
        FunctionContext context)
    {
        var logger = context.GetLogger("KafkaProducerFunction");

        if (string.IsNullOrEmpty(KafkaBroker) || string.IsNullOrEmpty(KafkaTopic))
        {
            logger.LogError("KAFKA_BROKER or KAFKA_TOPIC environment variables are not set.");
            return;
        }

        var config = new ProducerConfig
        {
            BootstrapServers = KafkaBroker,
            SecurityProtocol=SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.Plain,
            SaslUsername = Environment.GetEnvironmentVariable("ConfluentCloudUserName"),
            SaslPassword = Environment.GetEnvironmentVariable("ConfluentCloudPassword"),

            // Add additional Kafka producer configuration if needed
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var headers = new Headers();
            headers.Add("EventType", Encoding.UTF8.GetBytes("Kafka-ServiceBusEvent"));
            // Add more headers as needed

            // Construct the message value
            var messageValue = new
            {
                ordertime = 1497014222380,
                orderid = 18,
                itemid = "Item_184",
                address = new
                {
                    city = "Mountain View",
                    state = "CA",
                    zipcode = 94041
                }
            };

            // Convert the message value to a JSON string
            var messageValueJson = JsonConvert.SerializeObject(messageValue);

            // Produce a message to the topic with headers
            var message = new Message<Null, string>
            {
                Value = messageValueJson,
                Headers = headers
            };

            logger.LogInformation($"Producing Kafka event with headers: {message.Value}");

            producer.ProduceAsync(KafkaTopic, message)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        logger.LogError($"Error producing message: {task.Exception.Message}");
                    }
                    else
                    {
                        logger.LogInformation($"Produced message to: {task.Result.TopicPartitionOffset}");
                    }
                })
                .Wait();
        }
    }
}
