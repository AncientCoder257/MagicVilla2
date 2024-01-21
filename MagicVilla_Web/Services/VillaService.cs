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

    public Task<T> GetAllAsync<T>()
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/VillaAPI/GetVillas"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/VillaAPI/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaCreateDto dto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/VillaAPI/CreateVilla"
        });
    }

    public Task<T> UpdateAsync<T>(VillaUpdateDto dto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/VillaAPI?id="+dto.Id
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/VillaAPI/"+id
        });
    }
}