using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIM_MappingFunctions_Worker.Models
{

        public class OutputMessageQueue
        {
            public string instanceId { get; set; }
            public string Content { get; set; }
        }
        public class InputMessageQueue
        {
            public string instanceId { get; set; }
            public string Content { get; set; }
            public string WorkerName { get; set; }
        }
    
}
