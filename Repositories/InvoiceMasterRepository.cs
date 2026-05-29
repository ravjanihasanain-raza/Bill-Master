using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class InvoiceMasterRepository : IInvoiceMaster
    {
        private readonly ApplicationDBContext _dbContext;

        public InvoiceMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ SAVE
        //public async Task<ResponseResult> SaveFullInvoice(InvoiceWithItemsDto data)
        //{
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        var invoice = data.Invoice;

        //        // ✅ VALIDATIONS
        //        if (invoice.ClientMasterId <= 0)
        //            return new ResponseResult("Fail", "Client required");

        //        if (data.Items == null || data.Items.Count == 0)
        //            return new ResponseResult("Fail", "Items required");

        //        // ✅ INVOICE NUMBER
        //        var last = await _dbContext.InvoiceMasters
        //            .OrderByDescending(x => x.Id)
        //            .FirstOrDefaultAsync();

        //        int next = 1;
        //        if (last != null)
        //        {
        //            int.TryParse(last.InvoiceNo.Replace("INV-", ""), out next);
        //            next++;
        //        }

        //        invoice.InvoiceNo = $"INV-{next:D4}";
        //        invoice.CreatedAt = DateTime.Now;
        //        invoice.Total = invoice.GrossAmount + invoice.GstAmount;

        //        // ✅ SAVE MASTER
        //        _dbContext.InvoiceMasters.Add(invoice);
        //        await _dbContext.SaveChangesAsync();

        //        // ✅ SAVE ITEMS
        //        foreach (var item in data.Items)
        //        {
        //            item.InvoiceMasterId = invoice.Id;
        //            item.TaxableValue = item.Qty * item.Rate;
        //            item.Total = item.TaxableValue + item.GstAmount;

        //            _dbContext.InvoiceItems.Add(item);
        //        }

        //        await _dbContext.SaveChangesAsync();

        //        // ✅ COMMIT
        //        await transaction.CommitAsync();

        //        return new ResponseResult("OK", new
        //        {
        //            invoiceId = invoice.Id,
        //            invoiceNo = invoice.InvoiceNo
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        return new ResponseResult("Fail", ex.Message);
        //    }
        //}


        public async Task<ResponseResult> SaveFullInvoice(InvoiceWithItemsDto data)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var invoice = data.Invoice;

                // ✅ VALIDATION
                if (invoice.ClientMasterId <= 0)
                    return new ResponseResult("Fail", "Client required");

                if (data.Items == null || !data.Items.Any())
                    return new ResponseResult("Fail", "Items required");

                // ✅ GENERATE INVOICE NUMBER
                var last = await _dbContext.InvoiceMasters
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                int next = 1;
                if (last != null)
                {
                    int.TryParse(last.InvoiceNo.Replace("INV-", ""), out next);
                    next++;
                }

                invoice.InvoiceNo = $"INV-{next:D4}";
                invoice.CreatedAt = DateTime.Now;

                // ✅ SAVE INVOICE MASTER
                await _dbContext.InvoiceMasters.AddAsync(invoice);
                await _dbContext.SaveChangesAsync();

                decimal totalGross = 0;
                decimal totalGst = 0;


                // ===============================
                // ✅ SAVE ITEMS
                // ===============================
                foreach (var item in data.Items)
                {
                    var product = await _dbContext.ProductMasters
    .FirstOrDefaultAsync(p => p.Id == item.ProductMasterId);

                    if (product == null)
                        throw new Exception("Invalid product");

                    decimal gstRate = product.Gst;

                    item.InvoiceMasterId = invoice.Id;
                    item.TaxableValue = item.Qty * item.Rate;
                    item.GstAmount = item.TaxableValue * (gstRate / 100);
                    item.Total = item.TaxableValue + item.GstAmount;

                    totalGross += item.TaxableValue;
                    totalGst += item.GstAmount;

                    await _dbContext.InvoiceItems.AddAsync(item);
                }

                invoice.GrossAmount = totalGross;
                invoice.GstAmount = totalGst;
                invoice.Total = totalGross + totalGst;

                await _dbContext.SaveChangesAsync();

                // ===============================
                // 🔥 STOCK DEDUCTION (MAIN LOGIC)
                // ===============================
                foreach (var item in data.Items)
                {
                    decimal requiredQty = item.Qty;

                    var inwardStocks = await _dbContext.InwardStocks
                        .Include(x => x.StockUseds)
                        .Include(x => x.PurchaseItem)
                        .Where(x => x.PurchaseItem.ProductMasterId == item.ProductMasterId)
                        .OrderBy(x => x.InwardDate)
                        .ToListAsync();

                    foreach (var inward in inwardStocks)
                    {
                        var usedQty = inward.StockUseds.Sum(x => x.Qty);
                        var availableQty = inward.Qty - usedQty;

                        if (availableQty <= 0)
                            continue;

                        var consumeQty = Math.Min(availableQty, requiredQty);

                        var stockUsed = new StockUsed
                        {
                            InwardStockId = inward.Id,
                            Qty = consumeQty,
                            OutwardDate = DateTime.Now,

                            InvoiceMasterId = invoice.Id,

                            // IMPORTANT FIX
                            OutwardMasterId = null
                        };
                        await _dbContext.StockUseds.AddAsync(stockUsed);

                        requiredQty -= consumeQty;

                        if (requiredQty <= 0)
                            break;
                    }

                    // ❌ STOCK NOT ENOUGH
                    if (requiredQty > 0)
                        throw new Exception($"Stock not available for productId: {item.ProductMasterId}");
                }

                await _dbContext.SaveChangesAsync();

                // ✅ COMMIT
                await transaction.CommitAsync();

                return new ResponseResult("OK", new
                {
                    invoiceId = invoice.Id,
                    invoiceNo = invoice.InvoiceNo
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseResult(
                    "Fail",
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }


        // ✅ LIST
        public async Task<ResponseResult> ListInvoice()
        {
            try
            {
                var data = await _dbContext.InvoiceMasters
                    .Include(i => i.ClientMaster)
                    .Include(i => i.StaffMaster)
                    .Include(i => i.InvoicePayments)
                    .Select(i => new
                    {
                        i.Id,
                        i.InvoiceNo,
                        i.InvoiceDate,

                        // IMPORTANT
                        i.ClientMasterId,
                        i.GrossAmount,
                        i.GstAmount,

                        i.Total,

                        ClientName = i.ClientMaster!.BusinessName,
                        StaffName = i.StaffMaster!.FullName,

                        PaidAmount = i.InvoicePayments
        .Sum(p => (decimal?)p.Amount) ?? 0
                    })
                    .ToListAsync();

                // ✅ map status cleanly AFTER query (better control)
                var result = data.Select(i => new InvoiceListDto
                {
                    Id = i.Id,
                    InvoiceNo = i.InvoiceNo,
                    InvoiceDate = i.InvoiceDate,

                    // ✅ IMPORTANT
                    ClientMasterId = i.ClientMasterId,

                    GrossAmount = i.GrossAmount,
                    GstAmount = i.GstAmount,

                    Total = i.Total,

                    ClientName = i.ClientName,
                    StaffName = i.StaffName,

                    PaidAmount = i.PaidAmount,
                    PendingAmount = i.Total - i.PaidAmount,

                    // ✅ IMPORTANT
                    Status = (i.Total - i.PaidAmount) <= 0
        ? "Paid"
        : "Pending"
                }).ToList();

                return new ResponseResult("OK", result);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }


        // ✅ DETAIL
        public async Task<ResponseResult> DetailInvoice(int id)
        {
            try
            {
                var data = await _dbContext.InvoiceMasters
                    .Include(x => x.ClientMaster)
                    .Include(x => x.InvoiceItems)
                        .ThenInclude(i => i.ProductMaster)
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.InvoiceNo,
                        x.InvoiceDate,
                        x.PONumber,
                        x.PODate,
                        x.GrossAmount,
                        x.GstAmount,
                        x.Total,
                        x.ClientMasterId,
                       

                        // ✅ CLIENT FULL
                        Client = new
                        {
                            x.ClientMaster.BusinessName,
                            x.ClientMaster.Address,
                            x.ClientMaster.ContactNo,
                            x.ClientMaster.Email,
                            x.ClientMaster.GstIN,
                            x.ClientMaster.State,
                            x.ClientMaster.StateCode
                        },

                        // ✅ ITEMS FULL
                        InvoiceItems = x.InvoiceItems.Select(i => new
                        {
                            Name = i.ProductMaster.Name,
                            HSN = i.HSNCode,
                            Qty = i.Qty,
                            Unit = i.Unit,
                            Price = i.Rate,
                            Taxable = i.TaxableValue,
                            Gst = 0,
                            GstAmount = i.GstAmount,
                            Total = i.Total
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "No record");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }


        // ✅ UPDATE
        public async Task<ResponseResult> UpdateInvoice(InvoiceWithItemsDto data)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var invoice = data.Invoice;

                var existing = await _dbContext.InvoiceMasters
                    .Include(x => x.InvoiceItems)
                    .FirstOrDefaultAsync(x => x.Id == invoice.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Record not found");

                // ===============================
                // 🔥 STEP 1: STOCK ROLLBACK
                // ===============================
                var oldStock = await _dbContext.StockUseds
                    .Where(x => x.InvoiceMasterId == invoice.Id)
                    .ToListAsync();

                _dbContext.StockUseds.RemoveRange(oldStock);

                // ===============================
                // 🔥 STEP 2: DELETE OLD ITEMS
                // ===============================
                if (existing.InvoiceItems.Any())
                    _dbContext.InvoiceItems.RemoveRange(existing.InvoiceItems);

                await _dbContext.SaveChangesAsync();

                // ===============================
                // 🔥 STEP 3: UPDATE MASTER
                // ===============================
                existing.ClientMasterId = invoice.ClientMasterId;
                existing.InvoiceDate = invoice.InvoiceDate;
                existing.StaffMasterId = invoice.StaffMasterId;

                decimal totalGross = 0;
                decimal totalGst = 0;

                // ===============================
                // 🔥 STEP 4: ADD NEW ITEMS
                // ===============================
                foreach (var item in data.Items)
                {
                    var product = await _dbContext.ProductMasters
                        .FirstOrDefaultAsync(p => p.Id == item.ProductMasterId);

                    if (product == null)
                        throw new Exception("Invalid product");

                    decimal gstRate = product.Gst;

                    item.InvoiceMasterId = existing.Id;
                    item.TaxableValue = item.Qty * item.Rate;
                    item.GstAmount = item.TaxableValue * (gstRate / 100);
                    item.Total = item.TaxableValue + item.GstAmount;

                    totalGross += item.TaxableValue;
                    totalGst += item.GstAmount;

                    await _dbContext.InvoiceItems.AddAsync(item);
                }

                existing.GrossAmount = totalGross;
                existing.GstAmount = totalGst;
                existing.Total = totalGross + totalGst;

                await _dbContext.SaveChangesAsync();

                // ===============================
                // 🔥 STEP 5: APPLY NEW STOCK
                // ===============================
                foreach (var item in data.Items)
                {
                    decimal requiredQty = item.Qty;

                    var inwardStocks = await _dbContext.InwardStocks
                        .Include(x => x.StockUseds)
                        .Include(x => x.PurchaseItem)
                        .Where(x => x.PurchaseItem.ProductMasterId == item.ProductMasterId)
                        .OrderBy(x => x.InwardDate)
                        .ToListAsync();

                    foreach (var inward in inwardStocks)
                    {
                        var usedQty = inward.StockUseds.Sum(x => x.Qty);
                        var availableQty = inward.Qty - usedQty;

                        if (availableQty <= 0)
                            continue;

                        var consumeQty = Math.Min(availableQty, requiredQty);

                        var stockUsed = new StockUsed
                        {
                            InwardStockId = inward.Id,
                            Qty = consumeQty,
                            OutwardDate = DateTime.Now,

                            InvoiceMasterId = existing.Id,

                            // IMPORTANT
                            OutwardMasterId = null
                        };

                        await _dbContext.StockUseds.AddAsync(stockUsed);

                        requiredQty -= consumeQty;

                        if (requiredQty <= 0)
                            break;
                    }

                    if (requiredQty > 0)
                        throw new Exception($"Stock not enough for productId: {item.ProductMasterId}");
                }

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ResponseResult("OK", "Invoice updated with stock recalculation");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseResult("Fail",
                    ex.InnerException?.Message ?? ex.Message);
            }
        }


        // ✅ DELETE
        public async Task<ResponseResult> DeleteInvoice(int id)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var record = await _dbContext.InvoiceMasters
                    .Include(x => x.InvoiceItems)
                    .Include(x => x.InvoicePayments)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (record == null)
                    return new ResponseResult("Fail", "Record not found");

                // 🔥 STOCK ROLLBACK
                var stockUsedList = await _dbContext.StockUseds
                    .Where(x => record.InvoiceItems
                        .Select(i => i.ProductMasterId)
                        .Contains(x.InwardStock.PurchaseItem.ProductMasterId))
                    .ToListAsync();

                _dbContext.StockUseds.RemoveRange(stockUsedList);

                // DELETE child
                if (record.InvoiceItems.Any())
                    _dbContext.InvoiceItems.RemoveRange(record.InvoiceItems);

                if (record.InvoicePayments.Any())
                    _dbContext.InvoicePayments.RemoveRange(record.InvoicePayments);

                // DELETE parent
                _dbContext.InvoiceMasters.Remove(record);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResponseResult("OK", "Deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ResponseResult("Fail",
                    ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}