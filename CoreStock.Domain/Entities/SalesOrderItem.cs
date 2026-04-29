namespace CoreStock.Domain.Entities;

public class SalesOrderItem : BaseEntity
{
    public int SalesOrderId { get; set; }
    public SalesOrder SalesOrder { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    /// <summary>Average cost at time of sale (for COGS).</summary>
    public decimal UnitCost { get; set; }
    public decimal LineRevenue => Quantity * UnitPrice;
    public decimal LineCost => Quantity * UnitCost;
    public decimal LineProfit => LineRevenue - LineCost;
}
