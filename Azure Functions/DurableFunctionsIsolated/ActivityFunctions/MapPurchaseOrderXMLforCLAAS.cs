using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using Azure.Storage.Blobs.Models;
using System.Globalization;


namespace PIMMappingFunctions.ActivityFunctions
{
    [DurableTask(nameof(MapPurchaseOrderXMLforCLAAS))]
    public class MapPurchaseOrderXMLforCLAAS : TaskActivity<string, string>
    {
        private readonly ILogger<MapPurchaseOrderXMLforCLAAS> _logger;
       

        public MapPurchaseOrderXMLforCLAAS(
            ILogger<MapPurchaseOrderXMLforCLAAS> logger) // activites have access to DI.
        {
            _logger = logger;
           
        }

        public async override Task<string> RunAsync(TaskActivityContext context, string input)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = input;

            var outputfile = "Test";


            return outputfile;
        }

    }
}
