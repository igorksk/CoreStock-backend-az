using CoreStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreStock.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Brand> Brands { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Product> Products { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseItem> PurchaseItems { get; }
    DbSet<SalesOrder> SalesOrders { get; }
    DbSet<SalesOrderItem> SalesOrderItems { get; }
    DbSet<StockMovement> StockMovements { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
