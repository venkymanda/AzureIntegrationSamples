
using System.Threading.Tasks;

namespace InprocessSample.Services
{
    public interface ISampleService
    {

        public  Task<string> HttpSendTest(string xmlinput);
    }
       
}
