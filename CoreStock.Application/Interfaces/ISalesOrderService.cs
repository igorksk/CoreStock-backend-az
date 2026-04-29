using CoreStock.Application.DTOs.Orders;

namespace CoreStock.Application.Interfaces;

public interface ISalesOrderService
{
    Task<IEnumerable<SalesOrderDto>> GetAllAsync();
    Task<SalesOrderDto?> GetByIdAsync(int id);
    Task<SalesOrderDto> CreateAsync(CreateSalesOrderDto dto);
    Task<SalesOrderDto> UpdateStatusAsync(int id, string status);
    Task<SalesOrderDto> CancelAsync(int id);
}
