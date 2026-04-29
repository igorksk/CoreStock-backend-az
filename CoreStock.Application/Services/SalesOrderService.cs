using CoreStock.Application.DTOs.Orders;
using CoreStock.Application.Interfaces;
using CoreStock.Domain.Entities;
using CoreStock.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Application.Services;

public class SalesOrderService : ISalesOrderService
{
    private readonly IAppDbContext _context;

    public SalesOrderService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SalesOrderDto>> GetAllAsync()
    {
        var orders = await _context.SalesOrders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return orders.Select(MapToDto);
    }

    public async Task<SalesOrderDto?> GetByIdAsync(int id)
    {
        var order = await _context.SalesOrders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<SalesOrderDto> CreateAsync(CreateSalesOrderDto dto)
    {
        // Validate stock availability first
        foreach (var item in dto.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId)
                ?? throw new KeyNotFoundException($"Product {item.ProductId} not found.");
            if (product.StockQuantity < item.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{product.Name}'. Available: {product.StockQuantity}, requested: {item.Quantity}.");
        }

        var number = await GenerateOrderNumberAsync();

        var order = new SalesOrder
        {
            OrderNumber = number,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            OrderDate = dto.OrderDate,
            Status = OrderStatus.Confirmed,
            Notes = dto.Notes
        };

        // Build line items with cost captured at point of sale (snapshot avg cost)
        foreach (var itemDto in dto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId)!;
            order.Items.Add(new SalesOrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                UnitCost = product!.AverageCost
            });
        }

        order.TotalRevenue = order.Items.Sum(i => i.Quantity * i.UnitPrice);
        order.TotalCost = order.Items.Sum(i => i.Quantity * i.UnitCost);

        _context.SalesOrders.Add(order);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Deduct stock
        await DeductStockAsync(order);

        return await GetByIdAsync(order.Id) ?? throw new InvalidOperationException("Order not found.");
    }

    public async Task<SalesOrderDto> UpdateStatusAsync(int id, string status)
    {
        var order = await _context.SalesOrders.FindAsync(id)
            ?? throw new KeyNotFoundException($"Order {id} not found.");

        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            throw new ArgumentException($"Invalid status '{status}'.");

        order.Status = newStatus;
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(CancellationToken.None);

        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Order not found.");
    }

    public async Task<SalesOrderDto> CancelAsync(int id)
    {
        var order = await _context.SalesOrders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id)
            ?? throw new KeyNotFoundException($"Order {id} not found.");

        if (order.Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled.");
        if (order.Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel a delivered order.");

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;

        // Return stock
        foreach (var item in order.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) continue;

            var prevQty = product.StockQuantity;
            product.StockQuantity += item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            _context.StockMovements.Add(new StockMovement
            {
                ProductId = item.ProductId,
                Type = MovementType.Return,
                Quantity = item.Quantity,
                QuantityBefore = prevQty,
                QuantityAfter = product.StockQuantity,
                Reference = order.OrderNumber,
                MovementDate = DateTime.UtcNow,
                Notes = "Order cancellation — stock returned"
            });
        }

        await _context.SaveChangesAsync(CancellationToken.None);
        return await GetByIdAsync(id) ?? throw new InvalidOperationException("Order not found.");
    }

    private async Task DeductStockAsync(SalesOrder order)
    {
        foreach (var item in order.Items)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) continue;

            var prevQty = product.StockQuantity;
            product.StockQuantity -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            _context.StockMovements.Add(new StockMovement
            {
                ProductId = item.ProductId,
                Type = MovementType.Sale,
                Quantity = item.Quantity,
                QuantityBefore = prevQty,
                QuantityAfter = product.StockQuantity,
                Reference = order.OrderNumber,
                MovementDate = order.OrderDate,
                Notes = $"Sale to {order.CustomerName ?? "customer"}"
            });
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var count = await _context.SalesOrders.CountAsync();
        return $"SO-{DateTime.UtcNow:yyyyMM}-{count + 1:D4}";
    }

    private static SalesOrderDto MapToDto(SalesOrder o) => new()
    {
        Id = o.Id,
        OrderNumber = o.OrderNumber,
        CustomerName = o.CustomerName,
        CustomerEmail = o.CustomerEmail,
        OrderDate = o.OrderDate,
        Status = o.Status,
        Notes = o.Notes,
        TotalRevenue = o.TotalRevenue,
        TotalCost = o.TotalCost,
        Items = o.Items.Select(i => new SalesOrderItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.Product?.Name ?? string.Empty,
            ProductSKU = i.Product?.SKU ?? string.Empty,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            UnitCost = i.UnitCost
        }).ToList()
    };
}
