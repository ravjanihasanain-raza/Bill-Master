using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public StockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ LOW STOCK METHOD
        public async Task<ResponseResult> GetLowStockProducts()
        {
            try
            {
                var data = await _dbContext.ProductMasters
                    .Select(p => new
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        MinimumStock = p.MinimumStock,

                        TotalInward = _dbContext.InwardStocks
                            .Where(i => i.PurchaseItem.ProductMasterId == p.Id)
                            .Sum(i => (decimal?)i.Qty) ?? 0,

                        TotalUsed = _dbContext.StockUseds
                            .Where(s => s.InwardStock.PurchaseItem.ProductMasterId == p.Id)
                            .Sum(s => (decimal?)s.Qty) ?? 0
                    })
                    .ToListAsync();

                var result = data.Select(x => new
                {
                    x.ProductId,
                    x.ProductName,
                    x.MinimumStock,
                    AvailableStock = x.TotalInward - x.TotalUsed
                })
                .Where(x => x.AvailableStock <= x.MinimumStock)
                .ToList();

                return new ResponseResult("OK", result);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
        public async Task<ResponseResult> GetStockHistory(int productId)
        {
            try
            {
                // 🔹 INWARD (Stock In)
                var inward = await _dbContext.InwardStocks
                    .Where(x => x.PurchaseItem.ProductMasterId == productId)
                    .Select(x => new
                    {
                        Date = x.InwardDate,
                        Type = "IN",
                        Qty = x.Qty,
                        Ref = "Purchase",
                        InwardId = x.Id
                    })
                    .ToListAsync();

                // 🔹 OUTWARD (Stock Out via Invoice)
                var outward = await _dbContext.StockUseds
                    .Where(x => x.InwardStock.PurchaseItem.ProductMasterId == productId)
                    .Select(x => new
                    {
                        Date = x.OutwardDate,
                        Type = "OUT",
                        Qty = x.Qty,
                        Ref = "Invoice",
                        InwardId = x.InwardStockId
                    })
                    .ToListAsync();

                // 🔹 MERGE + SORT
                var ledger = inward
                    .Concat(outward)
                    .OrderBy(x => x.Date)
                    .ToList();

                return new ResponseResult("OK", ledger);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }

}