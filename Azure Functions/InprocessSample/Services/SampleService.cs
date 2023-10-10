using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Logging;
using System.Linq;
using System.Globalization;
using System.Net.Http;

namespace InprocessSample.Services
{
    public class SampleService : ISampleService
    {
        private HttpClient _httpClient;
        private ILogger<SampleService> _logger;
       

        public SampleService(IHttpClientFactory clientfactory,ILogger<SampleService> logger)
        {
            _httpClient = clientfactory.CreateClient();
            _logger = logger;
            
        }

      

        public async Task<string> HttpSendTest(string xmlinput)
        {
            _logger.LogInformation("test log");
            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://www.google.com"));
            return response.StatusCode.ToString();

        }
    }
}
