using MarketPlaceApi.Dtos;
using MarketPlaceApi.Entities;
using MarketPlaceApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarketPlaceApi.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public JWTService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public async Task<string> GenerateToken(LoginDto loginDto)
    {
        Guid guid = Guid.NewGuid();
        UserEntity role = await _userRepository.Get(loginDto.UserName);

        string secretKey = _config.GetSection("Jwt:Key").Value ?? throw new InvalidDataException("JWT SecretKey");
        string issuer = _config.GetSection("Jwt:Issuer").Value ?? throw new InvalidDataException("JWT Issuer");
        string audience = _config.GetSection("Jwt:Audience").Value ?? throw new InvalidDataException("JWT audience");

        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {

            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, guid.ToString()),
                new Claim(ClaimTypes.Role, role.Role)
            }),
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
