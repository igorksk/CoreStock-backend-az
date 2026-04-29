namespace CoreStock.Application.DTOs.Reports;

public class FinancialSummaryDto
{
    public decimal TotalRevenue { get; set; }
    public decimal TotalCost { get; set; }
    public decimal GrossProfit => TotalRevenue - TotalCost;
    public decimal GrossMarginPct => TotalRevenue > 0
        ? Math.Round(GrossProfit / TotalRevenue * 100, 2) : 0;
    public decimal TotalPurchaseExpenses { get; set; }
    public int TotalOrders { get; set; }
    public int TotalPurchases { get; set; }
    public decimal InventoryValue { get; set; }
}

public class DailySummaryDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public decimal Profit => Revenue - Cost;
    public int OrderCount { get; set; }
}

public class MonthlySummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMM yyyy");
    public decimal Revenue { get; set; }
    public decimal Cost { get; set; }
    public decimal Profit => Revenue - Cost;
    public decimal PurchaseExpenses { get; set; }
    public int OrderCount { get; set; }
    public int PurchaseCount { get; set; }
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalProfit { get; set; }
}

public class InventoryReportDto
{
    public decimal TotalInventoryValue { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockCount { get; set; }
    public int OutOfStockCount { get; set; }
    public List<InventoryItemDto> Items { get; set; } = new();
}

public class InventoryItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public decimal AverageCost { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal InventoryValue { get; set; }
    public string StockStatus { get; set; } = string.Empty;
}
