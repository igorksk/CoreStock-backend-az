namespace CoreStock.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    /// <summary>Standard purchase price (reference). Actual cost tracked via avg cost.</summary>
    public decimal PurchasePrice { get; set; }

    /// <summary>Retail selling price.</summary>
    public decimal SellingPrice { get; set; }

    /// <summary>Current weighted-average cost per unit.</summary>
    public decimal AverageCost { get; set; }

    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
    public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
