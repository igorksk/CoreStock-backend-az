namespace CoreStock.Application.DTOs.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal AverageCost { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public bool IsLowStock => StockQuantity <= MinStockLevel;
    public decimal InventoryValue => StockQuantity * AverageCost;
    public decimal Margin => SellingPrice > 0
        ? Math.Round((SellingPrice - AverageCost) / SellingPrice * 100, 2)
        : 0;
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int MinStockLevel { get; set; } = 5;
}

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int MinStockLevel { get; set; }
}
