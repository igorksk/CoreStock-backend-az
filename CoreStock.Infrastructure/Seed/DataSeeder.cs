using CoreStock.Domain.Entities;
using CoreStock.Domain.Enums;
using CoreStock.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Infrastructure.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        // ── Categories ──────────────────────────────────────────────────────────
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "CPU",           Description = "Central Processing Units" },
            new() { Id = 2, Name = "GPU",           Description = "Graphics Processing Units" },
            new() { Id = 3, Name = "RAM",           Description = "Memory Modules" },
            new() { Id = 4, Name = "SSD",           Description = "Solid State Drives" },
            new() { Id = 5, Name = "HDD",           Description = "Hard Disk Drives" },
            new() { Id = 6, Name = "Motherboard",   Description = "Mainboards" },
            new() { Id = 7, Name = "PSU",           Description = "Power Supply Units" },
            new() { Id = 8, Name = "Cooling",       Description = "CPU & Case Cooling" },
            new() { Id = 9, Name = "Case",          Description = "PC Chassis" },
        };

        // ── Brands ──────────────────────────────────────────────────────────────
        var brands = new List<Brand>
        {
            new() { Id = 1, Name = "Intel",     Country = "USA" },
            new() { Id = 2, Name = "AMD",       Country = "USA" },
            new() { Id = 3, Name = "NVIDIA",    Country = "USA" },
            new() { Id = 4, Name = "Samsung",   Country = "South Korea" },
            new() { Id = 5, Name = "Corsair",   Country = "USA" },
            new() { Id = 6, Name = "ASUS",      Country = "Taiwan" },
            new() { Id = 7, Name = "MSI",       Country = "Taiwan" },
            new() { Id = 8, Name = "Seagate",   Country = "USA" },
            new() { Id = 9, Name = "be quiet!", Country = "Germany" },
            new() { Id = 10, Name = "Noctua",   Country = "Austria" },
        };

        // ── Suppliers ───────────────────────────────────────────────────────────
        var suppliers = new List<Supplier>
        {
            new() { Id = 1, Name = "TechDistrib Ltd",    ContactName = "Alex Morrison",   Email = "alex@techdistrib.com",   Phone = "+1-800-100-2000" },
            new() { Id = 2, Name = "ComponentHub Inc",   ContactName = "Sara Chen",       Email = "sara@componenthub.com",  Phone = "+1-800-200-3000" },
            new() { Id = 3, Name = "ProParts EU",        ContactName = "Klaus Weber",     Email = "k.weber@proparts.eu",    Phone = "+49-30-99887766" },
        };

        context.Categories.AddRange(categories);
        context.Brands.AddRange(brands);
        context.Suppliers.AddRange(suppliers);
        await context.SaveChangesAsync();

        // ── Products ─────────────────────────────────────────────────────────────
        var products = new List<Product>
        {
            // CPUs
            new() { Id=1,  Name="Intel Core i9-14900K",  SKU="CPU-INT-001", CategoryId=1, BrandId=1,  PurchasePrice=399.00m, SellingPrice=549.99m, AverageCost=399.00m, StockQuantity=18, MinStockLevel=5 },
            new() { Id=2,  Name="Intel Core i7-14700K",  SKU="CPU-INT-002", CategoryId=1, BrandId=1,  PurchasePrice=289.00m, SellingPrice=399.99m, AverageCost=289.00m, StockQuantity=25, MinStockLevel=5 },
            new() { Id=3,  Name="AMD Ryzen 9 7950X",     SKU="CPU-AMD-001", CategoryId=1, BrandId=2,  PurchasePrice=499.00m, SellingPrice=679.99m, AverageCost=499.00m, StockQuantity=12, MinStockLevel=5 },
            new() { Id=4,  Name="AMD Ryzen 5 7600X",     SKU="CPU-AMD-002", CategoryId=1, BrandId=2,  PurchasePrice=179.00m, SellingPrice=249.99m, AverageCost=179.00m, StockQuantity=30, MinStockLevel=8 },
            // GPUs
            new() { Id=5,  Name="NVIDIA RTX 4090",       SKU="GPU-NVD-001", CategoryId=2, BrandId=3,  PurchasePrice=1299.00m, SellingPrice=1699.99m, AverageCost=1299.00m, StockQuantity=8,  MinStockLevel=3 },
            new() { Id=6,  Name="NVIDIA RTX 4070 Ti",    SKU="GPU-NVD-002", CategoryId=2, BrandId=3,  PurchasePrice=649.00m,  SellingPrice=849.99m,  AverageCost=649.00m,  StockQuantity=14, MinStockLevel=5 },
            new() { Id=7,  Name="AMD Radeon RX 7900 XTX",SKU="GPU-AMD-003", CategoryId=2, BrandId=2,  PurchasePrice=799.00m,  SellingPrice=1049.99m, AverageCost=799.00m,  StockQuantity=10, MinStockLevel=4 },
            new() { Id=8,  Name="AMD Radeon RX 7600",    SKU="GPU-AMD-004", CategoryId=2, BrandId=2,  PurchasePrice=219.00m,  SellingPrice=299.99m,  AverageCost=219.00m,  StockQuantity=22, MinStockLevel=8 },
            // RAM
            new() { Id=9,  Name="Corsair Vengeance 32GB DDR5", SKU="RAM-COR-001", CategoryId=3, BrandId=5,  PurchasePrice=89.00m,  SellingPrice=129.99m, AverageCost=89.00m,  StockQuantity=40, MinStockLevel=10 },
            new() { Id=10, Name="Corsair Vengeance 16GB DDR5", SKU="RAM-COR-002", CategoryId=3, BrandId=5,  PurchasePrice=49.00m,  SellingPrice=74.99m,  AverageCost=49.00m,  StockQuantity=3,  MinStockLevel=10 },
            new() { Id=11, Name="Samsung 32GB DDR5-5600",      SKU="RAM-SAM-001", CategoryId=3, BrandId=4,  PurchasePrice=99.00m,  SellingPrice=139.99m, AverageCost=99.00m,  StockQuantity=20, MinStockLevel=8  },
            // SSDs
            new() { Id=12, Name="Samsung 980 Pro 2TB NVMe",   SKU="SSD-SAM-001", CategoryId=4, BrandId=4,  PurchasePrice=109.00m, SellingPrice=159.99m, AverageCost=109.00m, StockQuantity=35, MinStockLevel=10 },
            new() { Id=13, Name="Samsung 870 EVO 1TB SATA",   SKU="SSD-SAM-002", CategoryId=4, BrandId=4,  PurchasePrice=64.00m,  SellingPrice=99.99m,  AverageCost=64.00m,  StockQuantity=28, MinStockLevel=8  },
            new() { Id=14, Name="Corsair MP600 Pro 2TB NVMe", SKU="SSD-COR-001", CategoryId=4, BrandId=5,  PurchasePrice=119.00m, SellingPrice=169.99m, AverageCost=119.00m, StockQuantity=0,  MinStockLevel=5  },
            // HDDs
            new() { Id=15, Name="Seagate Barracuda 4TB",      SKU="HDD-SEA-001", CategoryId=5, BrandId=8,  PurchasePrice=59.00m,  SellingPrice=89.99m,  AverageCost=59.00m,  StockQuantity=20, MinStockLevel=5  },
            // Motherboards
            new() { Id=16, Name="ASUS ROG Maximus Z790",      SKU="MBD-ASU-001", CategoryId=6, BrandId=6,  PurchasePrice=449.00m, SellingPrice=599.99m, AverageCost=449.00m, StockQuantity=10, MinStockLevel=3  },
            new() { Id=17, Name="MSI MAG X670E Tomahawk",     SKU="MBD-MSI-001", CategoryId=6, BrandId=7,  PurchasePrice=299.00m, SellingPrice=399.99m, AverageCost=299.00m, StockQuantity=4,  MinStockLevel=5  },
            // PSUs
            new() { Id=18, Name="be quiet! Dark Power 850W",  SKU="PSU-BEQ-001", CategoryId=7, BrandId=9,  PurchasePrice=129.00m, SellingPrice=179.99m, AverageCost=129.00m, StockQuantity=16, MinStockLevel=5  },
            new() { Id=19, Name="Corsair RM1000x 1000W",      SKU="PSU-COR-001", CategoryId=7, BrandId=5,  PurchasePrice=149.00m, SellingPrice=199.99m, AverageCost=149.00m, StockQuantity=12, MinStockLevel=4  },
            // Cooling
            new() { Id=20, Name="Noctua NH-D15 CPU Cooler",   SKU="CLG-NOC-001", CategoryId=8, BrandId=10, PurchasePrice=74.00m,  SellingPrice=109.99m, AverageCost=74.00m,  StockQuantity=2,  MinStockLevel=5  },
            new() { Id=21, Name="Corsair iCUE H150i Elite",   SKU="CLG-COR-001", CategoryId=8, BrandId=5,  PurchasePrice=119.00m, SellingPrice=169.99m, AverageCost=119.00m, StockQuantity=18, MinStockLevel=5  },
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // ── Purchases ────────────────────────────────────────────────────────────
        var purchases = GeneratePurchases(suppliers, products);
        context.Purchases.AddRange(purchases);
        await context.SaveChangesAsync();

        // ── Stock Movements for purchases ─────────────────────────────────────
        var purchaseMovements = GeneratePurchaseMovements(purchases);
        context.StockMovements.AddRange(purchaseMovements);

        // ── Sales Orders ─────────────────────────────────────────────────────
        var salesOrders = GenerateSalesOrders(products);
        context.SalesOrders.AddRange(salesOrders);
        await context.SaveChangesAsync();

        // ── Stock Movements for sales ──────────────────────────────────────────
        var saleMovements = GenerateSaleMovements(salesOrders);
        context.StockMovements.AddRange(saleMovements);
        await context.SaveChangesAsync();
    }

    // ────────────────────────────────────────────────────────────────────────────
    private static List<Purchase> GeneratePurchases(List<Supplier> suppliers, List<Product> products)
    {
        var rng = new Random(42);
        var purchases = new List<Purchase>();
        var id = 1;
        var now = DateTime.UtcNow;

        // 6 months of purchases
        for (int m = 5; m >= 0; m--)
        {
            var baseDate = now.AddMonths(-m);
            for (int w = 0; w < 2; w++)
            {
                var date = baseDate.AddDays(-(rng.Next(1, 14)));
                var supplier = suppliers[rng.Next(suppliers.Count)];
                var itemCount = rng.Next(3, 7);
                var selectedProducts = products.OrderBy(_ => rng.Next()).Take(itemCount).ToList();

                var items = selectedProducts.Select(p => new PurchaseItem
                {
                    ProductId = p.Id,
                    Quantity = rng.Next(5, 30),
                    UnitCost = p.PurchasePrice * (decimal)(0.90 + rng.NextDouble() * 0.10)
                }).ToList();

                purchases.Add(new Purchase
                {
                    Id = id++,
                    PurchaseNumber = $"PO-{date:yyyyMM}-{id:D4}",
                    SupplierId = supplier.Id,
                    OrderDate = date,
                    ReceivedDate = date.AddDays(3),
                    Status = PurchaseStatus.Received,
                    TotalAmount = items.Sum(i => i.Quantity * i.UnitCost),
                    Notes = "Regular stock replenishment",
                    Items = items
                });
            }
        }

        return purchases;
    }

    private static List<StockMovement> GeneratePurchaseMovements(List<Purchase> purchases)
    {
        var movements = new List<StockMovement>();
        foreach (var purchase in purchases)
        {
            foreach (var item in purchase.Items)
            {
                movements.Add(new StockMovement
                {
                    ProductId = item.ProductId,
                    Type = MovementType.Purchase,
                    Quantity = item.Quantity,
                    QuantityBefore = 0,
                    QuantityAfter = item.Quantity,
                    Reference = purchase.PurchaseNumber,
                    MovementDate = purchase.OrderDate,
                    Notes = "Seeded purchase"
                });
            }
        }
        return movements;
    }

    private static List<SalesOrder> GenerateSalesOrders(List<Product> products)
    {
        var rng = new Random(99);
        var customers = new[]
        {
            ("Build Masters LLC",       "orders@buildmasters.com"),
            ("PC Galaxy Store",         "procurement@pcgalaxy.com"),
            ("TechNest Retail",         "buy@technest.com"),
            ("HomeOffice Pro",          "supplies@homeofficepro.com"),
            ("GameRig Arena",           "parts@gamerigarena.com"),
            ("DataCenter Solutions",    "hardware@dcsolutions.com"),
        };

        var orders = new List<SalesOrder>();
        var id = 1;
        var now = DateTime.UtcNow;

        for (int m = 5; m >= 0; m--)
        {
            var baseDate = now.AddMonths(-m);
            var ordersThisMonth = rng.Next(8, 16);
            for (int i = 0; i < ordersThisMonth; i++)
            {
                var date = new DateTime(baseDate.Year, baseDate.Month,
                    rng.Next(1, DateTime.DaysInMonth(baseDate.Year, baseDate.Month) + 1),
                    rng.Next(8, 18), rng.Next(0, 60), 0, DateTimeKind.Utc);

                var customer = customers[rng.Next(customers.Length)];
                var itemCount = rng.Next(1, 5);
                var selectedProducts = products
                    .Where(p => p.StockQuantity > 0)
                    .OrderBy(_ => rng.Next())
                    .Take(itemCount)
                    .ToList();

                if (!selectedProducts.Any()) continue;

                var items = selectedProducts.Select(p =>
                {
                    var qty = rng.Next(1, Math.Min(4, p.StockQuantity) + 1);
                    return new SalesOrderItem
                    {
                        ProductId = p.Id,
                        Quantity = qty,
                        UnitPrice = p.SellingPrice * (decimal)(0.95 + rng.NextDouble() * 0.10),
                        UnitCost = p.AverageCost
                    };
                }).ToList();

                var totalRevenue = items.Sum(i => i.Quantity * i.UnitPrice);
                var totalCost = items.Sum(i => i.Quantity * i.UnitCost);

                var statuses = new[] { OrderStatus.Delivered, OrderStatus.Delivered, OrderStatus.Shipped, OrderStatus.Confirmed };
                orders.Add(new SalesOrder
                {
                    Id = id++,
                    OrderNumber = $"SO-{date:yyyyMM}-{id:D4}",
                    CustomerName = customer.Item1,
                    CustomerEmail = customer.Item2,
                    OrderDate = date,
                    Status = m == 0 ? statuses[rng.Next(statuses.Length)] : OrderStatus.Delivered,
                    TotalRevenue = totalRevenue,
                    TotalCost = totalCost,
                    Notes = "Seeded order",
                    Items = items
                });
            }
        }

        return orders;
    }

    private static List<StockMovement> GenerateSaleMovements(List<SalesOrder> orders)
    {
        var movements = new List<StockMovement>();
        foreach (var order in orders)
        {
            foreach (var item in order.Items)
            {
                movements.Add(new StockMovement
                {
                    ProductId = item.ProductId,
                    Type = MovementType.Sale,
                    Quantity = item.Quantity,
                    QuantityBefore = 0,
                    QuantityAfter = 0,
                    Reference = order.OrderNumber,
                    MovementDate = order.OrderDate,
                    Notes = $"Sale to {order.CustomerName}"
                });
            }
        }
        return movements;
    }
}
