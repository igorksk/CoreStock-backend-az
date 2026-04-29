using CoreStock.Application.DTOs.Catalog;
using CoreStock.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : BaseController
{
    private readonly IAppDbContext _context;

    public CatalogController(IAppDbContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var cats = await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products.Count(p => p.IsActive)
            }).ToListAsync();
        return Ok(cats);
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _context.Brands
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Country = b.Country,
                ProductCount = b.Products.Count(p => p.IsActive)
            }).ToListAsync();
        return Ok(brands);
    }

    [HttpGet("suppliers")]
    public async Task<IActionResult> GetSuppliers()
    {
        var suppliers = await _context.Suppliers
            .Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactName = s.ContactName,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address
            }).ToListAsync();
        return Ok(suppliers);
    }
}
