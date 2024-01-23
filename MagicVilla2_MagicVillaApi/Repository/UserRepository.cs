using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private string secretKey;

    public UserRepository(ApplicationDbContext dbContext,
        IConfiguration configuration, UserManager<ApplicationUser> userManager,
        IMapper mapper, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }

    public bool IsUniqueUser(string username)
    {
        var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = _dbContext.ApplicationUsers
            .FirstOrDefault(u =>
            u.UserName.ToLower() == loginRequestDto.Username.ToLower()
        );

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
        
        if (user == null)
        {
            return new LoginResponseDto()
            {
                Token = "",
                User = null,
            };
        }

        var roles = await _userManager.GetRolesAsync(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDto loginResponseDto = new LoginResponseDto()
        {
            Token = tokenHandler.WriteToken(token),
            User = _mapper.Map<UserDto>(user),
        };
        return loginResponseDto;
    }

    public async Task<UserDto> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new ApplicationUser()
        {
            UserName = registrationRequestDto.UserName,
            Email = registrationRequestDto.UserName,
            NormalizedEmail = registrationRequestDto.UserName.ToUpper(),
            Name = registrationRequestDto.Name,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("customer"));
                }

                await _userManager.AddToRoleAsync(user, "admin");
                var userToReturn = _dbContext.ApplicationUsers
                    .FirstOrDefault(v => v.UserName == registrationRequestDto.UserName);
                return _mapper.Map<UserDto>(userToReturn);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        return new UserDto();
    }
}