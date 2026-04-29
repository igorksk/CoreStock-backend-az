using CoreStock.Domain.Enums;

namespace CoreStock.Domain.Entities;

public class StockMovement : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public MovementType Type { get; set; }
    public int Quantity { get; set; }
    public int QuantityBefore { get; set; }
    public int QuantityAfter { get; set; }
    public string Reference { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; }
    public string? Notes { get; set; }
}
