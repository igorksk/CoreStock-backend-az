using CoreStock.Application.DTOs.Purchases;
using CoreStock.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoreStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : BaseController
{
    private readonly IPurchaseService _purchaseService;

    public PurchasesController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var purchases = await _purchaseService.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var purchase = await _purchaseService.GetByIdAsync(id);
        return purchase == null ? NotFound() : Ok(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseDto dto)
    {
        try
        {
            var purchase = await _purchaseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = purchase.Id }, purchase);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPost("{id:int}/receive")]
    public async Task<IActionResult> Receive(int id)
    {
        try
        {
            var purchase = await _purchaseService.ConfirmReceiptAsync(id);
            return Ok(purchase);
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var purchase = await _purchaseService.CancelAsync(id);
            return Ok(purchase);
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
