using MarketPlaceApi.Dtos;
using MarketPlaceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketService _marketService;

    public MarketController(IMarketService marketService)
    {
        _marketService = marketService;
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(ItemDto item)
    {
        await _marketService.AddItem(item);
        return StatusCode(201);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> AddOrder(OrderDto order)
    {
        await _marketService.AddOrder(order);
        return StatusCode(201);
    }

    [HttpPut("orders/completed/{id}")]
    public async Task<IActionResult> UpdateOrderAsCompleted(int id)
    {
        await _marketService.UpdateOrderAsCompleted(id);
        return Ok();
    }

    [HttpGet("orders/")]
    public async Task<IActionResult> GetUserOrders(int id)
    {
        return Ok(await _marketService.GetUserOrders(id));
    }
}
