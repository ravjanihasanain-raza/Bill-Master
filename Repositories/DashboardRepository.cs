using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDBContext _db;

        public DashboardRepository(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<ResponseResult> GetDashboard(DashboardFilter filter)
        {
            try
            {
                var (fromDate, toDate) = ResolveDateRange(filter);

                var result = new DashboardResult
                {
                    AppliedFilter = filter.FilterType,
                    FilterFrom = fromDate,
                    FilterTo = toDate
                };

                await BuildKpiCards(result, fromDate, toDate);
                await BuildMonthlyCharts(result, fromDate, toDate);
                await BuildSalesByCategory(result, fromDate, toDate);
                await BuildPaymentStatusPie(result, fromDate, toDate);
                await BuildTopClients(result, fromDate, toDate);
                await BuildRecentInvoices(result, fromDate, toDate);

                return new ResponseResult("OK", result);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        private static (DateTime, DateTime) ResolveDateRange(DashboardFilter filter)
        {
            var today = DateTime.Today;

            if (filter.FilterType == "today")
                return (today, today.AddDays(1).AddTicks(-1));

            if (filter.FilterType == "month")
            {
                int year = filter.Year ?? today.Year;
                int month = filter.Month ?? today.Month;

                var fromDate = new DateTime(year, month, 1);
                var toDate = fromDate.AddMonths(1).AddTicks(-1);

                return (fromDate, toDate);
            }

            if (filter.FilterType == "custom" &&
                filter.FromDate.HasValue &&
                filter.ToDate.HasValue)
            {
                return (
                    filter.FromDate.Value,
                    filter.ToDate.Value.AddDays(1).AddTicks(-1)
                );
            }

            int selectedYear = filter.Year ?? today.Year;

            return (
                new DateTime(selectedYear, 1, 1),
                new DateTime(selectedYear, 12, 31, 23, 59, 59)
            );
        }

        private async Task BuildKpiCards(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            result.TotalRevenue = await _db.InvoiceMasters
                .Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                .SumAsync(x => (decimal?)x.Total) ?? 0;

            result.TotalInvoices = await _db.InvoiceMasters
                .CountAsync(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate);

            result.TotalPurchase = await _db.PurchaseMasters
                .Where(x => x.BillDate >= fromDate && x.BillDate <= toDate)
                .SumAsync(x => (decimal?)x.Total) ?? 0;

            result.TotalCollected = await _db.InvoicePayments
                .Where(x =>
                    x.InvoiceMaster != null &&
                    x.InvoiceMaster.InvoiceDate >= fromDate &&
                    x.InvoiceMaster.InvoiceDate <= toDate)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            result.TotalOutstanding =
                result.TotalRevenue - result.TotalCollected;

            result.TotalClients = await _db.ClientMasters.CountAsync();
            result.TotalProducts = await _db.ProductMasters.CountAsync();
        }

        private async Task BuildMonthlyCharts(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            result.MonthlyRevenue = await _db.InvoiceMasters
                .Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                .GroupBy(x => new { x.InvoiceDate.Year, x.InvoiceDate.Month })
                .Select(g => new MonthlyPoint
                {
                    Year = g.Key.Year,
                    MonthNo = g.Key.Month,
                    Month = g.Key.Month + "/" + g.Key.Year,
                    Amount = g.Sum(x => x.Total)
                })
                .ToListAsync();

            result.MonthlyPurchase = await _db.PurchaseMasters
                .Where(x => x.BillDate >= fromDate && x.BillDate <= toDate)
                .GroupBy(x => new { x.BillDate.Year, x.BillDate.Month })
                .Select(g => new MonthlyPoint
                {
                    Year = g.Key.Year,
                    MonthNo = g.Key.Month,
                    Month = g.Key.Month + "/" + g.Key.Year,
                    Amount = g.Sum(x => x.Total)
                })
                .ToListAsync();
        }

        private async Task BuildSalesByCategory(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            result.SalesByCategory = await (
                from ii in _db.InvoiceItems
                join im in _db.InvoiceMasters on ii.InvoiceMasterId equals im.Id
                join pm in _db.ProductMasters on ii.ProductMasterId equals pm.Id
                join pc in _db.ProductCategories on pm.ProductCategoryId equals pc.Id
                where im.InvoiceDate >= fromDate && im.InvoiceDate <= toDate
                group ii by pc.CategoryName into g
                select new ChartSlice
                {
                    Label = g.Key,
                    Value = g.Sum(x => x.Total)
                }
            ).ToListAsync();
        }

        private async Task BuildPaymentStatusPie(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            var data = await _db.InvoiceMasters
                .Where(x => x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate)
                .Select(x => new
                {
                    x.Total,
                    Paid = x.InvoicePayments.Sum(p => (decimal?)p.Amount) ?? 0
                })
                .ToListAsync();

            result.InvoicePaymentStatus = data
                .GroupBy(x =>
                    x.Paid >= x.Total ? "Paid" :
                    x.Paid > 0 ? "Partial" : "Unpaid")
                .Select(g => new ChartSlice
                {
                    Label = g.Key,
                    Value = g.Count()
                })
                .ToList();
        }

        private async Task BuildTopClients(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            result.TopClientsByRevenue = await (
                from im in _db.InvoiceMasters
                join cm in _db.ClientMasters on im.ClientMasterId equals cm.Id
                where im.InvoiceDate >= fromDate && im.InvoiceDate <= toDate
                group im by cm.BusinessName into g
                select new ChartSlice
                {
                    Label = g.Key,
                    Value = g.Sum(x => x.Total)
                }
            ).Take(5).ToListAsync();
        }

        private async Task BuildRecentInvoices(
            DashboardResult result,
            DateTime fromDate,
            DateTime toDate)
        {
            result.RecentInvoices = await (
                from im in _db.InvoiceMasters
                join cm in _db.ClientMasters on im.ClientMasterId equals cm.Id
                where im.InvoiceDate >= fromDate && im.InvoiceDate <= toDate
                select new RecentInvoiceDto
                {
                    Id = im.Id,
                    InvoiceNo = im.InvoiceNo,
                    ClientName = cm.BusinessName,
                    InvoiceDate = im.InvoiceDate,
                    Total = im.Total
                }
            ).Take(10).ToListAsync();
        }
    }
}