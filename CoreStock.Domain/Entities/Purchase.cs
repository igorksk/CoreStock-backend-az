using CoreStock.Domain.Enums;

namespace CoreStock.Domain.Entities;

public class Purchase : BaseEntity
{
    public string PurchaseNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Draft;
    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }

    public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}
