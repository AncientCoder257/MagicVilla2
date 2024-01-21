using System.Net;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    protected ApiResponse _apiResponse;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        this._apiResponse = new();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var loginResponse = await _userRepository.Login(loginRequestDto);

        if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Username or password is incorrect");
            return BadRequest(_apiResponse);
        }
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        _apiResponse.Result = loginResponse;
        return Ok(_apiResponse);
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto regRequestDto)
    {
        bool ifUserNameUnique = _userRepository.IsUniqueUser(regRequestDto.UserName);

        if (!ifUserNameUnique)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Username already exists");
            return BadRequest(_apiResponse);
        }

        var user = await _userRepository.Register(regRequestDto);

        if (user == null)
        {
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.IsSuccess = false;
            _apiResponse.ErrorMessages.Add("Error while registering");
            return BadRequest(_apiResponse);
        }
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.IsSuccess = true;
        return Ok(_apiResponse);
    }
    
}