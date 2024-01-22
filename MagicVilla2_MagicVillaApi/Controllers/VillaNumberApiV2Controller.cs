using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class VillaNumberApiV2Controller : ControllerBase
{
    private readonly ILogger<VillaAPIController> _logger;
    private readonly IVillaNumberRepository _villaNumberRepository;
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    protected ApiResponse _response;

    public VillaNumberApiV2Controller(
        ILogger<VillaAPIController> logger,
        IVillaNumberRepository villaNumberRepository, 
        IVillaRepository villaRepository,
        IMapper mapper
    )
    {
        _logger = logger;
        _villaNumberRepository = villaNumberRepository;
        _villaRepository = villaRepository;
        _mapper = mapper;
        this._response = new();
    }
    
    [MapToApiVersion("2.0")]
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "val1", "val2" };
    }
}