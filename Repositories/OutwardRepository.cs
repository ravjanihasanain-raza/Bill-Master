using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model; // 🔥 FIX: Removed wrong namespace
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bill_Master.Repositories
{
    public class OutwardRepository : IOutward
    {
        private readonly ApplicationDBContext _dbContext;

        public OutwardRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ SAVE OUTWARD
        public async Task<ResponseResult> SaveOutward(Outward outward)
        {
            try
            {
                // 🔥 Check Staff Exists
                var staffExists = await _dbContext.StaffMasters
                    .AnyAsync(x => x.Id == outward.StaffMasterId);

                if (!staffExists)
                {
                    return new ResponseResult("Fail", "Invalid Staff");
                }

                outward.CreatedAt = DateTime.Now;

                _dbContext.Outwards.Add(outward);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Outward saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ LIST OUTWARD WITH STAFF NAME
        public async Task<ResponseResult> ListOutward()
        {
            try
            {
                var data = await _dbContext.StockUseds
                    .Include(x => x.InvoiceMaster)
                        .ThenInclude(i => i.StaffMaster)
                    .Include(x => x.InwardStock)
                        .ThenInclude(i => i.PurchaseItem)
                            .ThenInclude(p => p.ProductMaster)
                    .GroupBy(x => x.InvoiceMasterId)
                    .Select(g => new
                    {
                        Id = g.First().InvoiceMasterId,

                        OutwardNo = "OUT-" + g.First().InvoiceMasterId,

                        StaffName = g.First().InvoiceMaster.StaffMaster.FullName,

                        Remark = "Generated From Invoice",

                        OutwardDate = g.First().OutwardDate,

                        TotalItems = g.Count(),

                        TotalQtyUsed = g.Sum(x => x.Qty),

                        Status = "Consumed"
                    })
                    .OrderByDescending(x => x.OutwardDate)
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ DETAIL BY ID
        public async Task<ResponseResult> DetailOutward(int id)
        {
            try
            {
                var stockData = await _dbContext.StockUseds
                    .Include(x => x.InvoiceMaster)
                        .ThenInclude(i => i.StaffMaster)
                    .Include(x => x.InwardStock)
                        .ThenInclude(i => i.PurchaseItem)
                            .ThenInclude(p => p.ProductMaster)
                    .Where(x => x.InvoiceMasterId == id)
                    .ToListAsync();

                if (!stockData.Any())
                    return new ResponseResult("Fail", "No record found");

                var first = stockData.First();

                var result = new
                {
                    Id = id,

                    OutwardNumber = "OUT-" + id,

                    StaffName = first.InvoiceMaster.StaffMaster.FullName,

                    ConsumptionDate = first.OutwardDate,

                    TotalItemsConsumed = stockData.Count,

                    TotalQtyUsed = stockData.Sum(x => x.Qty),

                    FinancialYear = "2026-2027",

                    Remark = "Generated from Invoice",

                    ConsumedItems = stockData.Select(x => new
                    {
                        ProductName = x.InwardStock.PurchaseItem.ProductMaster.Name,

                        BatchNo = x.InwardStock.BatchNo,

                        QtyUsed = x.Qty,

                        AvailableQty =
                            x.InwardStock.Qty -
                            x.InwardStock.StockUseds.Sum(s => s.Qty),

                        Unit = x.InwardStock.PurchaseItem.ProductMaster.Unit,

                        ConsumptionDate = x.OutwardDate
                    }).ToList()
                };

                return new ResponseResult("OK", result);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ DELETE
        public async Task<ResponseResult> DeleteOutward(int id)
        {
            try
            {
                var outward = await _dbContext.Outwards.FindAsync(id);

                if (outward == null)
                    return new ResponseResult("Fail", "Record not found");

                _dbContext.Outwards.Remove(outward);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ✅ UPDATE
        public async Task<ResponseResult> UpdateOutward(Outward outward)
        {
            try
            {
                var existing = await _dbContext.Outwards
                    .FirstOrDefaultAsync(x => x.Id == outward.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Record not found");

                // 🔥 Staff Validation
                var staffExists = await _dbContext.StaffMasters
                    .AnyAsync(x => x.Id == outward.StaffMasterId);

                if (!staffExists)
                    return new ResponseResult("Fail", "Invalid Staff");

                // 🔥 Update Fields (Do not update CreatedAt here)
                existing.StaffMasterId = outward.StaffMasterId;
                existing.Remark = outward.Remark;
                existing.OutwardDate = outward.OutwardDate;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
        //public async Task<ResponseResult> AutoFromInvoice(InvoiceDto dto)
        //{
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        foreach (var item in dto.Items)
        //        {
        //            decimal remainingQty = item.Qty;

        //            // 🔥 FIFO: oldest inward first
        //            var inwardStocks = await _dbContext.InwardStocks
        //                .Where(x => x.ProductMasterId == item.ProductId)
        //                .OrderBy(x => x.InwardDate)
        //                .ToListAsync();

        //            foreach (var inward in inwardStocks)
        //            {
        //                // available stock calculate
        //                var usedQty = await _dbContext.StockUseds
        //                    .Where(x => x.InwardStockId == inward.Id)
        //                    .SumAsync(x => (decimal?)x.Qty) ?? 0;

        //                var available = inward.Qty - usedQty;

        //                if (available <= 0)
        //                    continue;

        //                var deductQty = Math.Min(available, remainingQty);

        //                // 🔥 SAVE STOCK USED
        //                _dbContext.StockUseds.Add(new StockUsed
        //                {
        //                    InwardStockId = inward.Id,
        //                    Qty = deductQty,
        //                    OutwardDate = DateTime.Now,
        //                    InvoiceMasterId = dto.InvoiceId
        //                });

        //                remainingQty -= deductQty;

        //                if (remainingQty <= 0)
        //                    break;
        //            }

        //            // ❌ अगर stock कम है
        //            if (remainingQty > 0)
        //            {
        //                throw new Exception($"Insufficient stock for productId {item.ProductId}");
        //            }
        //        }

        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return new ResponseResult("OK", "Stock deducted using FIFO");
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return new ResponseResult("Fail", ex.Message);
        //    }
        //}
    }
}