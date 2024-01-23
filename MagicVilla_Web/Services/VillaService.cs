using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaService : BaseService, IVillaService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;

    public VillaService(
        IHttpClientFactory clientFactory, 
        IConfiguration configuration
        ) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
    }

    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/v1/VillaAPI/GetVillas",
            Token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/v1/VillaAPI/"+id,
            Token = token
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDto dto, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/v1/VillaAPI/CreateVilla",
            Token = token
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDto dto, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/v1/VillaAPI?id="+dto.Id,
            Token = token
        });
    }

    public Task<T> DeleteAsync<T>(int id, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/v1/VillaAPI/"+id,
            Token = token
        });
    }
}