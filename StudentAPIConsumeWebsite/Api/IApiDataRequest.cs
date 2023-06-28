using StudentAPIConsumeWebsite.Models;

namespace StudentAPIConsumeWebsite.Api
{
    public interface IApiDataRequest
    {
        Task<Response> ConsumeApi(string url, string content, string reqType, string? contentType);
    }
}
