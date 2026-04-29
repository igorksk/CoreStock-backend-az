using CoreStock.Application.DTOs.Purchases;
using CoreStock.Application.Interfaces;
using CoreStock.Domain.Entities;
using CoreStock.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IAppDbContext _context;

    public PurchaseService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PurchaseDto>> GetAllAsync()
    {
        var purchases = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items).ThenInclude(i => i.Product)
            .OrderByDescending(p => p.OrderDate)
            .ToListAsync();
        return purchases.Select(MapToDto);
    }

    public async Task<PurchaseDto?> GetByIdAsync(int id)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(p => p.Id == id);
        return purchase == null ? null : MapToDto(purchase);
    }

    public async Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto)
    {
        var number = await GeneratePurchaseNumberAsync();

        var purchase = new Purchase
        {
            PurchaseNumber = number,
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            Status = PurchaseStatus.Confirmed,
            Notes = dto.Notes,
            Items = dto.Items.Select(i => new PurchaseItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost
            }).ToList()
        };

        purchase.TotalAmount = purchase.Items.Sum(i => i.Quantity * i.UnitCost);

        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Immediately apply stock on creation (confirmed)
        await ApplyStockAndCostAsync(purchase);

        return await GetByIdAsync(purchase.Id) ?? throw new InvalidOperationException("Purchase not found.");
    }

    public async Task<PurchaseDto> ConfirmReceiptAsync(int id)
    {
        var purchase = await _context.Purchases
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new KeyNotFoundException($"Purchase {id} not found.");

        if (purchase.Status == PurchaseStatus.Received)
            throw new InvalidOperationException("Purchase already received.");

        purchase.Status = PurchaseStatus.Received;
        purchase.ReceivedDate = DateTime.UtcNow;
        purchase.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(CancellationToken.None);

        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Purchase not found.");
    }

    public async Task<PurchaseDto> CancelAsync(int id)
    {
        var purchase = await _context.Purchases.FindAsync(id)
            ?? throw new KeyNotFoundException($"Purchase {id} not found.");

        if (purchase.Status == PurchaseStatus.Received)
            throw new InvalidOperationException("Cannot cancel a received purchase.");

        purchase.Status = PurchaseStatus.Cancelled;
        purchase.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(CancellationToken.None);

        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Purchase not found.");
    }

    // Weighted-average cost update + stock movement
    private async Task ApplyStockAndCostAsync(Purchase purchase)
    {
        foreach (var item in purchase.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) continue;

            var prevQty = product.StockQuantity;

            // Weighted average cost recalculation
            var totalExistingValue = product.StockQuantity * product.AverageCost;
            var incomingValue = item.Quantity * item.UnitCost;
            var newQty = product.StockQuantity + item.Quantity;

            product.AverageCost = newQty > 0
                ? (totalExistingValue + incomingValue) / newQty
                : item.UnitCost;

            product.StockQuantity = newQty;
            product.UpdatedAt = DateTime.UtcNow;

            _context.StockMovements.Add(new StockMovement
            {
                ProductId = item.ProductId,
                Type = MovementType.Purchase,
                Quantity = item.Quantity,
                QuantityBefore = prevQty,
                QuantityAfter = newQty,
                Reference = purchase.PurchaseNumber,
                MovementDate = purchase.OrderDate,
                Notes = $"Purchase from supplier"
            });
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private async Task<string> GeneratePurchaseNumberAsync()
    {
        var count = await _context.Purchases.CountAsync();
        return $"PO-{DateTime.UtcNow:yyyyMM}-{count + 1:D4}";
    }

    private static PurchaseDto MapToDto(Purchase p) => new()
    {
        Id = p.Id,
        PurchaseNumber = p.PurchaseNumber,
        SupplierId = p.SupplierId,
        SupplierName = p.Supplier?.Name ?? string.Empty,
        OrderDate = p.OrderDate,
        ReceivedDate = p.ReceivedDate,
        Status = p.Status,
        Notes = p.Notes,
        TotalAmount = p.TotalAmount,
        Items = p.Items.Select(i => new PurchaseItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            ProductSKU = i.Product?.SKU ?? string.Empty,
            Quantity = i.Quantity,
            UnitCost = i.UnitCost
        }).ToList()
    };
}
