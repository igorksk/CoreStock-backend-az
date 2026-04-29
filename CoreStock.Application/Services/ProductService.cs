using CoreStock.Application.DTOs.Products;
using CoreStock.Application.Interfaces;
using CoreStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Application.Services;

public class ProductService : IProductService
{
    private readonly IAppDbContext _context;

    public ProductService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            BrandId = dto.BrandId,
            PurchasePrice = dto.PurchasePrice,
            SellingPrice = dto.SellingPrice,
            AverageCost = dto.PurchasePrice,
            MinStockLevel = dto.MinStockLevel,
            StockQuantity = 0
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(CancellationToken.None);

        return await GetByIdAsync(product.Id) ?? throw new InvalidOperationException("Product not found after creation.");
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product {id} not found.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.CategoryId = dto.CategoryId;
        product.BrandId = dto.BrandId;
        product.PurchasePrice = dto.PurchasePrice;
        product.SellingPrice = dto.SellingPrice;
        product.MinStockLevel = dto.MinStockLevel;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(CancellationToken.None);
        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Product not found after update.");
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id)
            ?? throw new KeyNotFoundException($"Product {id} not found.");
        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive && p.StockQuantity <= p.MinStockLevel)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    private static ProductDto MapToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        SKU = p.SKU,
        Description = p.Description,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.Name ?? string.Empty,
        BrandId = p.BrandId,
        BrandName = p.Brand?.Name ?? string.Empty,
        PurchasePrice = p.PurchasePrice,
        SellingPrice = p.SellingPrice,
        AverageCost = p.AverageCost,
        StockQuantity = p.StockQuantity,
        MinStockLevel = p.MinStockLevel
    };
}
