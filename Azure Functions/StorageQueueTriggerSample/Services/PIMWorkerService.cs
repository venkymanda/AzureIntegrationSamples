using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using PIM_MappingFunctions_Worker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace PIM_MappingFunctions_Worker.Services
{
    public class PIMWorkerService : IPIMWorkerService
    {
        private readonly ILogger<IPIMWorkerService> _logger;
        private readonly HttpClient _httpClient;

        public PIMWorkerService(ILogger<IPIMWorkerService> logger, IHttpClientFactory clientFactory)
        {

            _logger = logger;
            _httpClient = clientFactory.CreateClient();
        }


        public  async Task<string> SalesOrderConvertandUploadToBlob(List<string> localpo)
        {
            try
            {

                return "";

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
