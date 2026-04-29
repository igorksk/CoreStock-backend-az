namespace CoreStock.Domain.Entities;

public class PurchaseItem : BaseEntity
{
    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost => Quantity * UnitCost;
}
