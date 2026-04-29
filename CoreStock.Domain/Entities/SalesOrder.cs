using CoreStock.Domain.Enums;

namespace CoreStock.Domain.Entities;

public class SalesOrder : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Draft;
    public string? Notes { get; set; }

    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Profit => TotalRevenue - TotalCost;

    public ICollection<SalesOrderItem> Items { get; set; } = new List<SalesOrderItem>();
}
