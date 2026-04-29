using CoreStock.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoreStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : BaseController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("financial-summary")]
    public async Task<IActionResult> GetFinancialSummary(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var start = from ?? DateTime.UtcNow.AddMonths(-1);
        var end = to ?? DateTime.UtcNow;
        var summary = await _reportService.GetFinancialSummaryAsync(start, end);
        return Ok(summary);
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailySummaries(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var start = from ?? DateTime.UtcNow.AddDays(-30);
        var end = to ?? DateTime.UtcNow;
        var data = await _reportService.GetDailySummariesAsync(start, end);
        return Ok(data);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlySummaries([FromQuery] int? year)
    {
        var y = year ?? DateTime.UtcNow.Year;
        var data = await _reportService.GetMonthlySummariesAsync(y);
        return Ok(data);
    }

    [HttpGet("top-products")]
    public async Task<IActionResult> GetTopProducts(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int take = 10)
    {
        var start = from ?? DateTime.UtcNow.AddMonths(-3);
        var end = to ?? DateTime.UtcNow;
        var data = await _reportService.GetTopSellingProductsAsync(start, end, take);
        return Ok(data);
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventoryReport()
    {
        var report = await _reportService.GetInventoryReportAsync();
        return Ok(report);
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var now = DateTime.UtcNow;
        var thisMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var lastMonthStart = thisMonthStart.AddMonths(-1);
        var last30 = now.AddDays(-30);
        var yearStart = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var thisMo = await _reportService.GetFinancialSummaryAsync(thisMonthStart, now);
        var lastMo = await _reportService.GetFinancialSummaryAsync(lastMonthStart, thisMonthStart.AddSeconds(-1));
        var ytd = await _reportService.GetFinancialSummaryAsync(yearStart, now);
        var monthly = await _reportService.GetMonthlySummariesAsync(now.Year);
        var topProducts = await _reportService.GetTopSellingProductsAsync(yearStart, now, 5);
        var inventory = await _reportService.GetInventoryReportAsync();
        var daily = await _reportService.GetDailySummariesAsync(last30, now);

        return Ok(new
        {
            thisMonth = thisMo,
            lastMonth = lastMo,
            ytd,
            monthly,
            topProducts,
            inventory = new { inventory.TotalInventoryValue, inventory.LowStockCount, inventory.OutOfStockCount, inventory.TotalProducts },
            daily
        });
    }
}
