using MarketPlaceApi.Dtos;
using MarketPlaceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Controllers;

/// <summary>
/// Controller for managing market operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class MarketController : ControllerBase
{
    private readonly IMarketService _marketService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarketController"/> class.
    /// </summary>
    /// <param name="marketService">The market service.</param>
    public MarketController(IMarketService marketService)
    {
        _marketService = marketService;
    }

    /// <summary>
    /// Add an item to the market.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The status code indicating the result of the operation.</returns>
    [HttpPost("items")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddItem(ItemDto item)
    {
        await _marketService.AddItem(item);
        return StatusCode(201);
    }

    /// <summary>
    /// Add an order to the market.
    /// </summary>
    /// <param name="order">The order to add.</param>
    /// <returns>The status code indicating the result of the operation.</returns>
    [HttpPost("orders")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddOrder(OrderDto order)
    {
        await _marketService.AddOrder(order);
        return StatusCode(201);
    }

    /// <summary>
    /// Update an order as completed.
    /// </summary>
    /// <param name="id">The ID of the order to update.</param>
    /// <returns>The status code indicating the result of the operation.</returns>
    [HttpPut("orders/completed/{id}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateOrderAsCompleted(int id)
    {
        await _marketService.UpdateOrderAsCompleted(id);
        return Ok();
    }

    /// <summary>
    /// Get orders for a specific user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user's orders.</returns>
    [HttpGet("orders/{id}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
    public async Task<IActionResult> GetUserOrders(int id)
    {
        return Ok(await _marketService.GetUserOrders(id));
    }
}