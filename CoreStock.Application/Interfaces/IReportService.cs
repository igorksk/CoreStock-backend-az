using CoreStock.Application.DTOs.Reports;

namespace CoreStock.Application.Interfaces;

public interface IReportService
{
    Task<FinancialSummaryDto> GetFinancialSummaryAsync(DateTime from, DateTime to);
    Task<IEnumerable<DailySummaryDto>> GetDailySummariesAsync(DateTime from, DateTime to);
    Task<IEnumerable<MonthlySummaryDto>> GetMonthlySummariesAsync(int year);
    Task<IEnumerable<TopProductDto>> GetTopSellingProductsAsync(DateTime from, DateTime to, int take = 10);
    Task<InventoryReportDto> GetInventoryReportAsync();
}
