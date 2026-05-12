using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class InwardStockRepository : IInwardStock
    {
        private readonly ApplicationDBContext _dbContext;

        public InwardStockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE INWARD
        public async Task<ResponseResult> SaveInward(InwardStock inward)
        {
            try
            {
                var purchaseItem = await _dbContext.PurchaseItems
                    .Include(p => p.PurchaseMaster)
                    .FirstOrDefaultAsync(x => x.Id == inward.PurchaseItemId);

                if (purchaseItem == null)
                    return new ResponseResult("Fail", "Invalid Purchase Item");

                if (!await _dbContext.StaffMasters.AnyAsync(x => x.Id == inward.StaffUserId))
                    return new ResponseResult("Fail", "Invalid Staff");

                // Over inward check
                var totalInward = await _dbContext.InwardStocks
                    .Where(x => x.PurchaseItemId == inward.PurchaseItemId)
                    .SumAsync(x => (decimal?)x.Qty) ?? 0;

                if (totalInward + inward.Qty > purchaseItem.Qty)
                    return new ResponseResult("Fail", "Qty exceeds purchase");

                inward.ProductMasterId = purchaseItem.ProductMasterId;

                // ✅ SAVE INWARD
                _dbContext.InwardStocks.Add(inward);
                await _dbContext.SaveChangesAsync(); ;

                // ============================
                // 🔥 STOCK UPDATE LOGIC START
                // ============================

                var productId = purchaseItem.ProductMasterId;

                var stock = await _dbContext.Stocks
                    .FirstOrDefaultAsync(x => x.ProductMasterId == productId);

                if (stock == null)
                {
                    // new product stock
                    stock = new Stock
                    {
                        ProductMasterId = productId,
                        Qty = inward.Qty
                    };

                    _dbContext.Stocks.Add(stock);
                }
                else
                {
                    // existing stock update
                    stock.Qty += inward.Qty;
                }

                await _dbContext.SaveChangesAsync();

                // ============================
                // 🔥 STOCK UPDATE LOGIC END
                // ============================

                return new ResponseResult("OK", "Stock inward + stock updated");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST
        public async Task<ResponseResult> ListInward()
        {
            try
            {
                var data = await _dbContext.InwardStocks
                    .Include(i => i.StockUseds)
                    .Select(i => new
                    {
                        i.Id,
                        i.BatchNo,
                        i.Qty,

                        UsedQty = i.StockUseds.Sum(x => (decimal?)x.Qty) ?? 0,

                        AvailableQty = i.Qty - (i.StockUseds.Sum(x => (decimal?)x.Qty) ?? 0),

                        i.InwardDate,

                        Product = i.PurchaseItem.ProductMaster.Name,
                        BillNo = i.PurchaseItem.PurchaseMaster.BillNo,
                        Staff = i.StaffUser.FullName
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL
        public async Task<ResponseResult> DetailInward(int id)
        {
            try
            {
                var data = await _dbContext.InwardStocks
                    .Where(i => i.Id == id)
                    .Select(i => new
                    {
                        i.Id,
                        i.PurchaseItemId,
                        i.BatchNo,
                        i.Qty,
                        i.InwardDate,
                        i.StaffUserId,
                        i.Remark
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "Inward not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE
        public async Task<ResponseResult> UpdateInward(InwardStock inward)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existing = await _dbContext.InwardStocks
                    .Include(x => x.StockUseds)
                    .FirstOrDefaultAsync(x => x.Id == inward.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Inward not found");

                var purchaseItem = await _dbContext.PurchaseItems
                    .FirstOrDefaultAsync(x => x.Id == existing.PurchaseItemId);

                if (purchaseItem == null)
                    return new ResponseResult("Fail", "Invalid purchase item");

                // 🔥 USED QTY
                var usedQty = existing.StockUseds.Sum(x => x.Qty);

                // ❌ can't reduce below used
                if (inward.Qty < usedQty)
                    return new ResponseResult("Fail", $"Cannot reduce below used qty ({usedQty})");

                // 🔥 CORRECT TOTAL CHECK
                var totalOtherInward = await _dbContext.InwardStocks
                    .Where(x => x.PurchaseItemId == existing.PurchaseItemId && x.Id != inward.Id)
                    .SumAsync(x => (decimal?)x.Qty) ?? 0;

                if (totalOtherInward + inward.Qty > purchaseItem.Qty)
                    return new ResponseResult("Fail", "Qty exceeds purchase");

                // 🔥 STOCK ADJUST
                var productId = purchaseItem.ProductMasterId;
                var stock = await _dbContext.Stocks
                    .FirstOrDefaultAsync(x => x.ProductMasterId == productId);

                var diff = inward.Qty - existing.Qty;

                if (stock != null)
                    stock.Qty += diff;

                // 🔥 UPDATE
                existing.Qty = inward.Qty;
                existing.BatchNo = inward.BatchNo;
                existing.InwardDate = inward.InwardDate;
                existing.StaffUserId = inward.StaffUserId;
                existing.Remark = inward.Remark;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult("OK", "Updated successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DELETE
        public async Task<ResponseResult> DeleteInward(int id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (id <= 0)
                    return new ResponseResult("Fail", "Invalid inward id");

                var existing = await _dbContext.InwardStocks
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (existing == null)
                    return new ResponseResult("Fail", "Inward not found");

                // purchase item
                var purchaseItem = await _dbContext.PurchaseItems
                    .FirstOrDefaultAsync(x => x.Id == existing.PurchaseItemId);

                if (purchaseItem == null)
                    return new ResponseResult("Fail", "Invalid purchase item");

                // stock
                var stock = await _dbContext.Stocks
                    .FirstOrDefaultAsync(x => x.ProductMasterId == purchaseItem.ProductMasterId);

                if (stock == null)
                    return new ResponseResult("Fail", "Stock not found");

                // 🔥 SUBTRACT STOCK
                stock.Qty -= existing.Qty;

                if (stock.Qty < 0)
                    return new ResponseResult("Fail", "Stock cannot go negative");

                _dbContext.InwardStocks.Remove(existing);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult("OK", "Inward deleted + stock updated");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}