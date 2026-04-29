using CoreStock.Application.DTOs.Purchases;

namespace CoreStock.Application.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseDto>> GetAllAsync();
    Task<PurchaseDto?> GetByIdAsync(int id);
    Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto);
    Task<PurchaseDto> ConfirmReceiptAsync(int id);
    Task<PurchaseDto> CancelAsync(int id);
}
