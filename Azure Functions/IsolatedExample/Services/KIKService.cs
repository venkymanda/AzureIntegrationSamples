
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace KIKDocumentAPIs.Services
{
    public class KIKService : IKIKService
    {
        
        private readonly ILogger<IKIKService> _logger;
        private readonly HttpClient _httpClient;

        public  KIKService (ILogger<IKIKService> logger,IHttpClientFactory clientFactory)
        {
           
            _logger = logger;
            _httpClient = clientFactory.CreateClient();
        }

        public async Task<string> HttpSendTest(string xmlinput)
        {
            _logger.LogInformation("test log");
            var response=await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://www.google.com"));
            return response.StatusCode.ToString() ;

        }

      
    }
}
