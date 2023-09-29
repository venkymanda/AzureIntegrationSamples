using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KIKDocumentAPIs.Services
{
    public interface IKIKService
    {
        public Task<string> HttpSendTest(string xmlinput);
        
    }
}
