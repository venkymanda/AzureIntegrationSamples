using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMMappingFunctions.Models
{
    [Serializable]
    public class TaskInfo
    {
        public string callbackUrl;
        public string activity;
        public string requestbody;
        public string jsonresponse;
        public string xmlresponse;
        public bool isxml;
        public bool iserror;
        

       
    }
}
