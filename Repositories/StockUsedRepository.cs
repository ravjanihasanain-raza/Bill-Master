using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class StockUsedRepository : IStockUsed
    {
        private readonly ApplicationDBContext _dbContext;

        public StockUsedRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ SAVE
        public async Task<ResponseResult> SaveStockUsed(StockUsed stockUsed)
        {
            try
            {

                var inward = await _dbContext.InwardStocks
                    .Include(x => x.StockUseds)
                    .FirstOrDefaultAsync(x => x.Id == stockUsed.InwardStockId);

                if (inward == null)
                    return new ResponseResult("Fail", "Invalid InwardStock");

                var usedQty = inward.StockUseds.Sum(x => x.Qty);

                if (usedQty + stockUsed.Qty > inward.Qty)
                    return new ResponseResult("Fail", "Not enough stock available");

                // 🔥 InwardStock Exist Check
                var inwardExists = await _dbContext.InwardStocks
                    .AnyAsync(x => x.Id == stockUsed.InwardStockId);

                if (!inwardExists)
                    return new ResponseResult("Fail", "Invalid InwardStock");

                // 🔥 Outward Exist Check
                //var outwardExists = await _dbContext.Outwards
                //    .AnyAsync(x => x.Id == stockUsed.OutwardMasterId);

                //if (!outwardExists)
                //    return new ResponseResult("Fail", "Invalid Outward");

                _dbContext.StockUseds.Add(stockUsed);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Stock usage saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ LIST
        public async Task<ResponseResult> ListStockUsed()
        {
            try
            {
                var data = await _dbContext.StockUseds
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.Id,
                        x.InwardStockId,
                        x.Qty,
                        x.OutwardDate,
                        x.OutwardMasterId,
                        x.InvoiceMasterId
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult(
                    "Fail",
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        // ✅ DETAIL
        public async Task<ResponseResult> DetailStockUsed(int id)
        {
            try
            {
                var data = await _dbContext.StockUseds
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.InwardStockId,
                        x.Qty,
                        x.OutwardDate,
                        x.OutwardMasterId
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "Record not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ UPDATE
        public async Task<ResponseResult> UpdateStockUsed(StockUsed stockUsed)
        {
            try
            {
                var existing = await _dbContext.StockUseds
                    .FirstOrDefaultAsync(x => x.Id == stockUsed.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Record not found");

                // 🔥 Inward exists check
                var inwardExists = await _dbContext.InwardStocks
                    .AnyAsync(x => x.Id == stockUsed.InwardStockId);

                if (!inwardExists)
                    return new ResponseResult("Fail", "Invalid InwardStock");

                // 🔥 Outward exists check
                var outwardExists = await _dbContext.Outwards
                    .AnyAsync(x => x.Id == stockUsed.OutwardMasterId);

                if (!outwardExists)
                    return new ResponseResult("Fail", "Invalid Outward");

                existing.InwardStockId = stockUsed.InwardStockId;
                existing.Qty = stockUsed.Qty;
                existing.OutwardDate = stockUsed.OutwardDate;
                existing.OutwardMasterId = stockUsed.OutwardMasterId;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail",
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
        // ✅ DELETE
        public async Task<ResponseResult> DeleteStockUsed(int id)
        {
            try
            {
                var record = await _dbContext.StockUseds.FindAsync(id);

                if (record == null)
                    return new ResponseResult("Fail", "Record not found");

                _dbContext.StockUseds.Remove(record);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}