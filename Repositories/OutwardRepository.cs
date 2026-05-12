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
                var data = await _dbContext.Outwards
                    .Include(o => o.StaffMaster)
                    .Select(o => new
                    {
                        o.Id,
                        o.StaffMasterId,
                        StaffName = o.StaffMaster!.FullName,
                        o.Remark,
                        o.OutwardDate,
                        o.CreatedAt
                    })
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
                var data = await _dbContext.Outwards
                    .Include(o => o.StaffMaster)
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        o.StaffMasterId,
                        StaffName = o.StaffMaster!.FullName,
                        o.Remark,
                        o.OutwardDate,
                        o.CreatedAt
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
        public async Task<ResponseResult> AutoFromInvoice(InvoiceDto dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in dto.Items)
                {
                    decimal remainingQty = item.Qty;

                    // 🔥 FIFO: oldest inward first
                    var inwardStocks = await _dbContext.InwardStocks
                        .Where(x => x.ProductMasterId == item.ProductId)
                        .OrderBy(x => x.InwardDate)
                        .ToListAsync();

                    foreach (var inward in inwardStocks)
                    {
                        // available stock calculate
                        var usedQty = await _dbContext.StockUseds
                            .Where(x => x.InwardStockId == inward.Id)
                            .SumAsync(x => (decimal?)x.Qty) ?? 0;

                        var available = inward.Qty - usedQty;

                        if (available <= 0)
                            continue;

                        var deductQty = Math.Min(available, remainingQty);

                        // 🔥 SAVE STOCK USED
                        _dbContext.StockUseds.Add(new StockUsed
                        {
                            InwardStockId = inward.Id,
                            Qty = deductQty,
                            OutwardDate = DateTime.Now,
                            InvoiceMasterId = dto.InvoiceId
                        });

                        remainingQty -= deductQty;

                        if (remainingQty <= 0)
                            break;
                    }

                    // ❌ अगर stock कम है
                    if (remainingQty > 0)
                    {
                        throw new Exception($"Insufficient stock for productId {item.ProductId}");
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult("OK", "Stock deducted using FIFO");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}