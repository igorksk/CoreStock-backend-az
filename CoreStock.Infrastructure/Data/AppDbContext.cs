using CoreStock.Application.Interfaces;
using CoreStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.PurchasePrice).HasPrecision(18, 4);
            e.Property(p => p.SellingPrice).HasPrecision(18, 4);
            e.Property(p => p.AverageCost).HasPrecision(18, 4);
            e.HasIndex(p => p.SKU).IsUnique();
        });

        modelBuilder.Entity<PurchaseItem>(e =>
        {
            e.Property(p => p.UnitCost).HasPrecision(18, 4);
            e.Ignore(p => p.TotalCost);
        });

        modelBuilder.Entity<SalesOrderItem>(e =>
        {
            e.Property(p => p.UnitPrice).HasPrecision(18, 4);
            e.Property(p => p.UnitCost).HasPrecision(18, 4);
            e.Ignore(p => p.LineRevenue);
            e.Ignore(p => p.LineCost);
            e.Ignore(p => p.LineProfit);
        });

        modelBuilder.Entity<SalesOrder>(e =>
        {
            e.Property(p => p.TotalRevenue).HasPrecision(18, 4);
            e.Property(p => p.TotalCost).HasPrecision(18, 4);
            e.Ignore(p => p.Profit);
        });

        modelBuilder.Entity<Purchase>(e =>
        {
            e.Property(p => p.TotalAmount).HasPrecision(18, 4);
        });
    }
}
