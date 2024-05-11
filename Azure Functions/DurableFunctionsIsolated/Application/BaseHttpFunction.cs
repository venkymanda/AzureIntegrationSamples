using System.Net;
using System.Net.Http;
using System.Text;


namespace PIMMappingFunctions.Application
{
    public abstract class BaseHttpFunction
    {
        protected HttpResponseMessage BadRequestResult(string message)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message)
            };
        }

        protected HttpResponseMessage OkRequestResult(string result)
        {
           
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(result, Encoding.UTF8, @"application/json"),
            };
        }

    }
}
