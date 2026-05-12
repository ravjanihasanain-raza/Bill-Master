using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class InvoiceItemsRepository : IInvoiceItems
    {
        private readonly ApplicationDBContext _dbContext;

        public InvoiceItemsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ SAVE
        public async Task<ResponseResult> SaveInvoiceItem(InvoiceItems item)
        {
            try
            {
                // 🔥 Invoice Exists Check
                var invoiceExists = await _dbContext.InvoiceMasters
                    .AnyAsync(x => x.Id == item.InvoiceMasterId);

                if (!invoiceExists)
                    return new ResponseResult("Fail", "Invalid Invoice");

                // 🔥 Product Exists Check
                var productExists = await _dbContext.ProductMasters
                    .AnyAsync(x => x.Id == item.ProductMasterId);

                if (!productExists)
                    return new ResponseResult("Fail", "Invalid Product");

                // 🔥 BASIC VALIDATION
                if (item.Qty <= 0)
                    return new ResponseResult("Fail", "Quantity must be greater than 0");

                if (item.Rate <= 0)
                    return new ResponseResult("Fail", "Rate must be greater than 0");

                if (item.GstAmount < 0)
                    return new ResponseResult("Fail", "Gst amount cannot be negative");

                // 🔥 AUTO CALCULATIONS
                item.TaxableValue = item.Qty * item.Rate;
                item.Total = item.TaxableValue + item.GstAmount;

                _dbContext.InvoiceItems.Add(item);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Invoice item saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail",
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ✅ LIST
        // ✅ LIST
        public async Task<ResponseResult> ListInvoiceItems()
        {
            try
            {
                var data = await _dbContext.InvoiceItems
                    .Include(x => x.InvoiceMaster)
                    .Include(x => x.ProductMaster)
                    .Select(x => new
                    {
                        x.Id,
                        x.InvoiceMasterId,
                        InvoiceNo = x.InvoiceMaster!.InvoiceNo,
                        x.ProductMasterId,
                        ProductName = x.ProductMaster!.Name,   // 🔥 FIXED
                        x.HSNCode,
                        x.Qty,
                        x.Unit,
                        x.Rate,
                        x.TaxableValue,
                        x.GstAmount,
                        x.Total
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
        // ✅ DETAIL
        // ✅ DETAIL
        public async Task<ResponseResult> DetailInvoiceItem(int id)
        {
            try
            {
                var data = await _dbContext.InvoiceItems
                    .Include(x => x.InvoiceMaster)
                    .Include(x => x.ProductMaster)
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.InvoiceMasterId,
                        InvoiceNo = x.InvoiceMaster!.InvoiceNo,
                        x.ProductMasterId,
                        ProductName = x.ProductMaster!.Name,   // 🔥 FIXED
                        x.HSNCode,
                        x.Qty,
                        x.Unit,
                        x.Rate,
                        x.TaxableValue,
                        x.GstAmount,
                        x.Total
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
        public async Task<ResponseResult> UpdateInvoiceItem(InvoiceItems item)
        {
            try
            {
                var existing = await _dbContext.InvoiceItems
                    .FirstOrDefaultAsync(x => x.Id == item.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Record not found");

                // 🔥 Invoice Exists Check
                var invoiceExists = await _dbContext.InvoiceMasters
                    .AnyAsync(x => x.Id == item.InvoiceMasterId);

                if (!invoiceExists)
                    return new ResponseResult("Fail", "Invalid Invoice");

                // 🔥 Product Exists Check
                var productExists = await _dbContext.ProductMasters
                    .AnyAsync(x => x.Id == item.ProductMasterId);

                if (!productExists)
                    return new ResponseResult("Fail", "Invalid Product");

                // 🔥 BASIC VALIDATION
                if (item.Qty <= 0)
                    return new ResponseResult("Fail", "Quantity must be greater than 0");

                if (item.Rate <= 0)
                    return new ResponseResult("Fail", "Rate must be greater than 0");

                if (item.GstAmount < 0)
                    return new ResponseResult("Fail", "Gst amount cannot be negative");

                existing.InvoiceMasterId = item.InvoiceMasterId;
                existing.ProductMasterId = item.ProductMasterId;
                existing.HSNCode = item.HSNCode;
                existing.Qty = item.Qty;
                existing.Unit = item.Unit;
                existing.Rate = item.Rate;

                // 🔥 AUTO RECALCULATE
                existing.TaxableValue = item.Qty * item.Rate;
                existing.GstAmount = item.GstAmount;
                existing.Total = existing.TaxableValue + item.GstAmount;

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
        public async Task<ResponseResult> DeleteInvoiceItem(int id)
        {
            try
            {
                var record = await _dbContext.InvoiceItems.FindAsync(id);

                if (record == null)
                    return new ResponseResult("Fail", "Record not found");

                _dbContext.InvoiceItems.Remove(record);
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