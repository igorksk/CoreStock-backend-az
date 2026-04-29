using CoreStock.Application.DTOs.Reports;
using CoreStock.Application.Interfaces;
using CoreStock.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Application.Services;

public class ReportService : IReportService
{
    private readonly IAppDbContext _context;

    public ReportService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<FinancialSummaryDto> GetFinancialSummaryAsync(DateTime from, DateTime to)
    {
        var orders = await _context.SalesOrders
            .Where(o => o.OrderDate >= from && o.OrderDate <= to
                     && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var purchases = await _context.Purchases
            .Where(p => p.OrderDate >= from && p.OrderDate <= to
                     && p.Status != PurchaseStatus.Cancelled)
            .ToListAsync();

        var inventoryValue = await _context.Products
            .Where(p => p.IsActive)
            .SumAsync(p => (decimal)p.StockQuantity * p.AverageCost);

        return new FinancialSummaryDto
        {
            TotalRevenue = orders.Sum(o => o.TotalRevenue),
            TotalCost = orders.Sum(o => o.TotalCost),
            TotalPurchaseExpenses = purchases.Sum(p => p.TotalAmount),
            TotalOrders = orders.Count,
            TotalPurchases = purchases.Count,
            InventoryValue = inventoryValue
        };
    }

    public async Task<IEnumerable<DailySummaryDto>> GetDailySummariesAsync(DateTime from, DateTime to)
    {
        var orders = await _context.SalesOrders
            .Where(o => o.OrderDate >= from && o.OrderDate <= to
                     && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        return orders
            .GroupBy(o => o.OrderDate.Date)
            .OrderBy(g => g.Key)
            .Select(g => new DailySummaryDto
            {
                Date = g.Key,
                Revenue = g.Sum(o => o.TotalRevenue),
                Cost = g.Sum(o => o.TotalCost),
                OrderCount = g.Count()
            });
    }

    public async Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummariesAsync(int year)
    {
        var orders = await _context.SalesOrders
            .Where(o => o.OrderDate.Year == year && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var purchases = await _context.Purchases
            .Where(p => p.OrderDate.Year == year && p.Status != PurchaseStatus.Cancelled)
            .ToListAsync();

        var months = Enumerable.Range(1, 12);
        return months.Select(month => new MonthlySummaryDto
        {
            Year = year,
            Month = month,
            Revenue = orders.Where(o => o.OrderDate.Month == month).Sum(o => o.TotalRevenue),
            Cost = orders.Where(o => o.OrderDate.Month == month).Sum(o => o.TotalCost),
            PurchaseExpenses = purchases.Where(p => p.OrderDate.Month == month).Sum(p => p.TotalAmount),
            OrderCount = orders.Count(o => o.OrderDate.Month == month),
            PurchaseCount = purchases.Count(p => p.OrderDate.Month == month)
        });
    }

    public async Task<IEnumerable<TopProductDto>> GetTopSellingProductsAsync(DateTime from, DateTime to, int take = 10)
    {
        var items = await _context.SalesOrderItems
            .Include(i => i.Product).ThenInclude(p => p.Category)
            .Include(i => i.SalesOrder)
            .Where(i => i.SalesOrder.OrderDate >= from && i.SalesOrder.OrderDate <= to
                     && i.SalesOrder.Status != OrderStatus.Cancelled)
            .ToListAsync();

        return items
            .GroupBy(i => i.ProductId)
            .Select(g => new TopProductDto
            {
                ProductId = g.Key,
                ProductName = g.First().Product?.Name ?? string.Empty,
                SKU = g.First().Product?.SKU ?? string.Empty,
                CategoryName = g.First().Product?.Category?.Name ?? string.Empty,
                TotalQuantitySold = g.Sum(i => i.Quantity),
                TotalRevenue = g.Sum(i => i.Quantity * i.UnitPrice),
                TotalProfit = g.Sum(i => i.Quantity * i.UnitPrice - i.Quantity * i.UnitCost)
            })
            .OrderByDescending(p => p.TotalRevenue)
            .Take(take);
    }

    public async Task<InventoryReportDto> GetInventoryReportAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.IsActive)
            .ToListAsync();

        var items = products.Select(p => new InventoryItemDto
        {
            ProductId = p.Id,
            ProductName = p.Name,
            SKU = p.SKU,
            CategoryName = p.Category?.Name ?? string.Empty,
            BrandName = p.Brand?.Name ?? string.Empty,
            StockQuantity = p.StockQuantity,
            MinStockLevel = p.MinStockLevel,
            AverageCost = p.AverageCost,
            SellingPrice = p.SellingPrice,
            InventoryValue = p.StockQuantity * p.AverageCost,
            StockStatus = p.StockQuantity == 0 ? "Out of Stock"
                : p.StockQuantity <= p.MinStockLevel ? "Low Stock"
                : "In Stock"
        }).ToList();

        return new InventoryReportDto
        {
            TotalInventoryValue = items.Sum(i => i.InventoryValue),
            TotalProducts = items.Count,
            LowStockCount = items.Count(i => i.StockStatus == "Low Stock"),
            OutOfStockCount = items.Count(i => i.StockStatus == "Out of Stock"),
            Items = items
        };
    }
}
