using StudentAPIConsumeWebsite.Models;
using System.Text;

namespace StudentAPIConsumeWebsite.Api
{
    public class ApiDataRequest : IApiDataRequest
    {
        public async Task<Response> ConsumeApi(string url, string content, string reqType, string contentType)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;

                if (reqType.ToUpper() == "GET")
                {
                    response = await client.GetAsync(url);
                }
                else if (reqType.ToUpper() == "POST")
                {
                    if (contentType.ToUpper() == "XML")
                    {
                        var postContent = new StringContent(content, Encoding.UTF8, "application/xml");
                        response = await client.PostAsync(url, postContent);
                    }
                    else
                    {
                        var postContent = new StringContent(content, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(url, postContent);
                    }
                }
                else if (reqType.ToUpper() == "PUT")
                {
                    var putContent = new StringContent(content, Encoding.UTF8, "application/json");
                    response = await client.PutAsync(url, putContent);
                }
                else if (reqType.ToUpper() == "DELETE")
                {
                    response = await client.DeleteAsync(url);
                }
                else
                {
                    throw new ArgumentException("Invalid request type. Supported types: GET, POST, PUT, DELETE.");
                } 
                

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    Response apiResponse = new Response();
                    apiResponse.data = result;
                    apiResponse.HttpStatusCode = response.StatusCode;
                    return apiResponse;
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Response apiResponse = new Response();
                    apiResponse.data = result;
                    apiResponse.HttpStatusCode = response.StatusCode;
                    return apiResponse;
                }
            }
        }
    }
}
