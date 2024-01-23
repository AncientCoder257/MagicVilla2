using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;

    public AuthService(
        IHttpClientFactory clientFactory,
        IConfiguration configuration
    ) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
    }

    public Task<T> LoginAsync<T>(LoginRequestDto objToCreate)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = objToCreate,
            Url = villaUrl + "/v1/Users/login"
        });
    }

    public Task<T> RegisterAsync<T>(RegistrationRequestDto objToCreate)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = objToCreate,
            Url = villaUrl + "/v1/Users/Register"
        });
    }
}