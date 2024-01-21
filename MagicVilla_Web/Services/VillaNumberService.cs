using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services;

public class VillaNumberService : BaseService, IVillaNumberService
{
    private readonly IHttpClientFactory _clientFactory;
    private string villaUrl;

    public VillaNumberService(
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
            Url = villaUrl + "/VillaNumberApi/GetVillaNumbers"
        });
    }

    public Task<T> GetAsync<T>(int id)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.GET,
            Url = villaUrl + "/VillaNumberApi/"+id
        });
    }

    public Task<T> CreateAsync<T>(VillaNumberCreateDto dto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.POST,
            Data = dto,
            Url = villaUrl + "/VillaNumberApi/CreateVillaNumber"
        });
    }

    public Task<T> UpdateAsync<T>(VillaNumberUpdateDto dto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.PUT,
            Data = dto,
            Url = villaUrl + "/VillaNumberApi?id="+dto.VillaNo
        });
    }

    public Task<T> DeleteAsync<T>(int id)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticDetails.ApiType.DELETE,
            Url = villaUrl + "/VillaNumberApi/"+id
        });
    }
}