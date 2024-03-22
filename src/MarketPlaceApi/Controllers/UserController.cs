using MarketPlaceApi.Entities;
using MarketPlaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "administrator")]
    [HttpPost]
    public async Task<IActionResult> Add(User user)
    {
        await _userService.Add(user);
        return Created("User added", user);
    }
}