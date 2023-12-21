using System;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace ServiceBusTriggerFunction
{
    public class ServiceBusTrigger
    {
        private readonly ILogger<ServiceBusTrigger> _logger;
        private readonly HttpClient _httpClient;

        public ServiceBusTrigger(ILogger<ServiceBusTrigger> logger,
                                IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        [Function(nameof(ServiceBusTrigger))]
        public async Task<VoidResult> Run([ServiceBusTrigger("kafkacjvn", "kafkacjvn", Connection = "connectionstring")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            try
            {
                // Get Bearer token
                var accessToken = await GetAccessToken();

                if (accessToken != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
                }
                string apiUrl = Environment.GetEnvironmentVariable("D365URL") ?? "";
                // Create an anonymous object with the desired structure
                var data = new
                {
                    salesorder = new
                    {
                        Data = Encoding.UTF8.GetString(message.Body)
                    }
                };

                // Convert the anonymous object to a JSON string
                string jsonString = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message ID: {id} sent to D365 Succesfully", message.MessageId);
                }

                return new VoidResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message failed with  {ex}");

                throw new Exception(ex.Message);
            }
        }
        public struct VoidResult { }
        public async Task<AuthenticationResult> GetAccessToken()
        {
            var clientId = Environment.GetEnvironmentVariable("D365FO.ClientId");
            var clientSecret = Environment.GetEnvironmentVariable("D365FO.ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("D365FO.TenantId");
            var resourceUri = Environment.GetEnvironmentVariable("D365FO.ResourceUri");
            Uri uri = new Uri("https://login.microsoftonline.com/" + tenantId);

            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                                                    .WithClientSecret(clientSecret)
                                                    .WithAuthority(uri)
                                                    .Build();

            var authResult = await app.AcquireTokenForClient(
                                            new[] { $"{resourceUri}/.default" })

                                            .ExecuteAsync()
                                            .ConfigureAwait(false);

            return authResult;



        }
    }
}
