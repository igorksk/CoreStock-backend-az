using CoreStock.Domain.Enums;

namespace CoreStock.Application.DTOs.Purchases;

public class PurchaseDto
{
    public int Id { get; set; }
    public string PurchaseNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public PurchaseStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
}

public class PurchaseItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost => Quantity * UnitCost;
}

public class CreatePurchaseDto
{
    public int SupplierId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public List<CreatePurchaseItemDto> Items { get; set; } = new();
}

public class CreatePurchaseItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
