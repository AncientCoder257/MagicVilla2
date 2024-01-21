using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services;

public class BaseService : IBaseService
{
    public ApiResponse responseModel { get; set; }
    public IHttpClientFactory ClientFactory { get; set; }

    public BaseService(IHttpClientFactory clientFactory)
    {
        this.responseModel = new();
        this.ClientFactory = clientFactory;
    }
    public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = ClientFactory.CreateClient("MagicApi");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            switch (apiRequest.ApiType)
            {
                case StaticDetails.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case StaticDetails.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case StaticDetails.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage httpResponse = null;
            
            httpResponse = await client.SendAsync(message);

            var apiContent = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiContent);
                if (apiResponse.StatusCode == HttpStatusCode.BadRequest 
                    || apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    apiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(apiResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }
            }
            catch (Exception ex)
            {
                var exceptionRes = JsonConvert.DeserializeObject<T>(apiContent);
                return exceptionRes;
            }
            var apiRes = JsonConvert.DeserializeObject<T>(apiContent);
            return apiRes;
        }
        catch (Exception ex)
        {
            var dto = new ApiResponse
            {
                ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(dto);
            var apiResponse = JsonConvert.DeserializeObject<T>(res);
            return apiResponse;
        }
    }
}