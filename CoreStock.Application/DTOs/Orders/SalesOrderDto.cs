using CoreStock.Domain.Enums;

namespace CoreStock.Application.DTOs.Orders;

public class SalesOrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public string? Notes { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Profit => TotalRevenue - TotalCost;
    public decimal MarginPct => TotalRevenue > 0 ? Math.Round(Profit / TotalRevenue * 100, 2) : 0;
    public List<SalesOrderItemDto> Items { get; set; } = new();
}

public class SalesOrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitCost { get; set; }
    public decimal LineRevenue => Quantity * UnitPrice;
    public decimal LineCost => Quantity * UnitCost;
    public decimal LineProfit => LineRevenue - LineCost;
}

public class CreateSalesOrderDto
{
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public List<CreateSalesOrderItemDto> Items { get; set; } = new();
}

public class CreateSalesOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
