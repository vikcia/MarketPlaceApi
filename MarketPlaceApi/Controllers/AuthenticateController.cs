using MarketPlaceApi.Dtos;
using MarketPlaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly AuthenticateService _authenticateService;

    public AuthenticateController(AuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(LoginDto loginDto)
    {
        return Ok(await _authenticateService.CheckLoginData(loginDto));
    }
}
