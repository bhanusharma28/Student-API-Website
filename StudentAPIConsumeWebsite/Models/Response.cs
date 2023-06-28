using System.Net;

namespace StudentAPIConsumeWebsite.Models
{
    public class Response
    {
        public string data { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
