using CoreStock.Application.DTOs.Orders;
using CoreStock.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoreStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : BaseController
{
    private readonly ISalesOrderService _orderService;

    public OrdersController(ISalesOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSalesOrderDto dto)
    {
        try
        {
            var order = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
    {
        try
        {
            var order = await _orderService.UpdateStatusAsync(id, req.Status);
            return Ok(order);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var order = await _orderService.CancelAsync(id);
            return Ok(order);
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}

public record UpdateStatusRequest(string Status);
