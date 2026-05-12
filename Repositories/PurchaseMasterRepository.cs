using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;


namespace Bill_Master.Repositories
{
    public class PurchaseMasterRepository : IPurchaseMaster
    {
        private readonly ApplicationDBContext _dbContext;

        public PurchaseMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE PURCHASE
        public async Task<ResponseResult> SavePurchase(PurchaseMaster purchase)
        {
            try
            {
                // 🔴 BASIC VALIDATION
                if (purchase == null)
                    return new ResponseResult("Fail", "Purchase data is required");

                if (purchase.VendorId <= 0)
                    return new ResponseResult("Fail", "Vendor is required");

                if (string.IsNullOrWhiteSpace(purchase.BillNo))
                    return new ResponseResult("Fail", "Bill number is required");

                if (purchase.BillDate == default)
                    return new ResponseResult("Fail", "Bill date is required");

                if (purchase.Total <= 0)
                    return new ResponseResult("Fail", "Total amount must be greater than 0");

                if (purchase.PurchaseItems == null || !purchase.PurchaseItems.Any())
                    return new ResponseResult("Fail", "At least one item is required");

                // 🔴 DUPLICATE BILL CHECK
                var duplicate = await _dbContext.PurchaseMasters
                    .AnyAsync(x => x.BillNo == purchase.BillNo);

                if (duplicate)
                    return new ResponseResult("Fail", "Bill number already exists");

                // 🔴 FK VALIDATION
                var vendorExists = await _dbContext.Vendors
                    .AnyAsync(x => x.Id == purchase.VendorId);

                if (!vendorExists)
                    return new ResponseResult("Fail", "Invalid Vendor");

                // 🔴 ITEM VALIDATION
                foreach (var item in purchase.PurchaseItems)
                {
                    var stock = await _dbContext.Stocks
                        .FirstOrDefaultAsync(x => x.ProductMasterId == item.ProductMasterId);

                    if (stock != null)
                    {
                        stock.Qty += item.Qty;
                    }
                    else
                    {
                        await _dbContext.Stocks.AddAsync(new Stock
                        {
                            ProductMasterId = item.ProductMasterId,
                            Qty = item.Qty
                        });
                    }
                }

                purchase.CreatedAt = DateTime.Now;

                await _dbContext.PurchaseMasters.AddAsync(purchase);
                await _dbContext.SaveChangesAsync();

                // 🔥 IMPORTANT: PurchaseItems include karo
                var savedPurchase = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchaseItems)
                    .FirstOrDefaultAsync(p => p.Id == purchase.Id);

                return new ResponseResult("OK", savedPurchase);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST PURCHASES (FK INCLUDED)
        public async Task<ResponseResult> ListPurchase()
        {
            try
            {
                var data = await _dbContext.PurchaseMasters
                    .Include(p => p.Vendor)
                    .Include(p => p.PurchasePayments)
                    .Select(p => new
                    {
                        p.Id,
                        p.BillNo,
                        p.BillDate,

                        Vendor = p.Vendor != null ? p.Vendor.BusinessName : "",

                        Total = p.Total,
                        PurchaseItems = p.PurchaseItems.Select(i => new
                        {
                            i.Id,
                            i.ProductMasterId,
                            i.Qty,
                            i.Rate,
                            i.Amount,
                            i.Total
                        }),
                        // ✅ SAFE CALCULATIONS
                        PaidAmount = p.PurchasePayments
                            .Sum(x => (decimal?)x.Amount) ?? 0,

                        PendingAmount = p.Total -
                            (p.PurchasePayments.Sum(x => (decimal?)x.Amount) ?? 0),

                        Status =
                            (p.PurchasePayments.Sum(x => (decimal?)x.Amount) ?? 0) == 0
                                ? "Unpaid"
                                : (p.Total - (p.PurchasePayments.Sum(x => (decimal?)x.Amount) ?? 0)) == 0
                                    ? "Paid"
                                    : "Partial"
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL BY ID

        public async Task<ResponseResult> DetailPurchase(int id)
        {
            try
            {
                var data = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchaseItems) // 🔥 IMPORTANT
                    .Where(p => p.Id == id)
                    .Select(p => new
                    {
                        p.Id,
                        p.FinancialYearId,
                        p.BillNo,
                        p.BillDate,
                        p.GrossAmount,
                        p.GstAmount,
                        p.Total,
                        p.GstType,
                        p.EwayBillNo,
                        p.PlaceOfSupply,
                        p.TransportName,
                        p.TransportMobile,
                        p.VehicleNo,
                        p.VendorId,
                        p.StaffMasterId,

                        // 🔥 THIS IS THE FIX
                        PurchaseItems = p.PurchaseItems.Select(i => new
                        {
                            i.Id,
                            i.ProductMasterId,
                            i.Qty,
                            i.Rate,
                            i.Amount,
                            i.Total
                        })
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "Purchase not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE PURCHASE
        public async Task<ResponseResult> UpdatePurchase(PurchaseMaster request)
        {
            try
            {
                // 🔴 BASIC VALIDATION
                if (request == null || request.Id <= 0)
                    return new ResponseResult("Fail", "Invalid purchase data");

                var existingPurchase = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchaseItems)

                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (existingPurchase == null)
                    return new ResponseResult("Fail", "Purchase not found");

                // 🔴 DUPLICATE BILL CHECK
                var duplicate = await _dbContext.PurchaseMasters
                    .AnyAsync(x => x.BillNo == request.BillNo && x.Id != request.Id);

                if (duplicate)
                    return new ResponseResult("Fail", "Bill number already exists");

                // 🔴 UPDATE MASTER
                existingPurchase.VendorId = request.VendorId;
                existingPurchase.BillNo = request.BillNo;
                existingPurchase.BillDate = request.BillDate;
                existingPurchase.Total = request.Total;
                existingPurchase.GrossAmount = request.GrossAmount;
                existingPurchase.GstAmount = request.GstAmount;
                existingPurchase.GstType = request.GstType;
                existingPurchase.PlaceOfSupply = request.PlaceOfSupply;
                existingPurchase.TransportName = request.TransportName;
                existingPurchase.TransportMobile = request.TransportMobile;
                existingPurchase.VehicleNo = request.VehicleNo;
                existingPurchase.EwayBillNo = request.EwayBillNo;

                // ===============================
                // ⭐ ITEM UPDATE LOGIC (IMPORTANT)
                // ===============================

                var existingItems = existingPurchase.PurchaseItems.ToList();

                foreach (var item in request.PurchaseItems)
                {
                    // 🔍 Check if item already exists
                    var existingItem = existingItems
                        .FirstOrDefault(x => x.Id == item.Id);

                    if (existingItem != null)
                    {
                        // 🛑 CHECK: Is item used in InwardStock?
                        var isUsed = await _dbContext.InwardStocks
                            .AnyAsync(x => x.PurchaseItemId == existingItem.Id);

                        if (isUsed)
                        {
                            // ❌ BUSINESS RULE: cannot modify critical fields
                            if (existingItem.ProductMasterId != item.ProductMasterId)
                                return new ResponseResult("Fail",
                                    "Cannot change product of inwarded item");

                            // ✅ Only allow safe update
                            existingItem.Qty = item.Qty;
                            existingItem.Rate = item.Rate;
                        }
                        else
                        {
                            // ✅ FULL UPDATE allowed
                            existingItem.ProductMasterId = item.ProductMasterId;
                            existingItem.Qty = item.Qty;
                            existingItem.Rate = item.Rate;
                        }

                        // ✅ Recalculate
                        existingItem.Amount = item.Qty * item.Rate;
                        existingItem.Total = item.Total;
                    }
                    else
                    {
                        // ➕ NEW ITEM ADD
                        var newItem = new PurchaseItems
                        {
                            PurchaseMasterId = request.Id,
                            ProductMasterId = item.ProductMasterId,
                            Qty = item.Qty,
                            Rate = item.Rate,
                            Amount = item.Amount,
                            Total = item.Total
                        };

                        await _dbContext.PurchaseItems.AddAsync(newItem);
                    }
                }

                // ===============================
                // 🗑 HANDLE REMOVED ITEMS
                // ===============================

                foreach (var oldItem in existingPurchase.PurchaseItems)
                {
                    var isUsed = await _dbContext.InwardStocks
                        .AnyAsync(x => x.PurchaseItemId == oldItem.Id);

                    if (isUsed)
                    {
                        return new ResponseResult("Fail",
                            "Item already used in inward. Cannot update.");
                    }
                }

                // agar sab safe hai tab hi delete karo
                //_dbContext.PurchaseItems.RemoveRange(existingPurchase.PurchaseItems);

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Purchase updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult(
                    "Fail",
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        // ⭐ DELETE PURCHASE
        public async Task<ResponseResult> DeletePurchase(int id)
        {
            try
            {
                // 1. Include the related child records (Items & Payments)
                var existing = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchaseItems)
                    .Include(p => p.PurchasePayments)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (existing == null)
                    return new ResponseResult("Fail", "Purchase not found");

                // 2. Delete related Purchase Items first
                if (existing.PurchaseItems != null && existing.PurchaseItems.Any())
                {
                    _dbContext.PurchaseItems.RemoveRange(existing.PurchaseItems);
                }

                // 3. Delete related Purchase Payments first
                if (existing.PurchasePayments != null && existing.PurchasePayments.Any())
                {
                    _dbContext.PurchasePayments.RemoveRange(existing.PurchasePayments);
                }

                // 4. Finally, delete the Purchase Master
                _dbContext.PurchaseMasters.Remove(existing);

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Purchase deleted successfully");
            }
            catch (Exception ex)
            {
                // 🔥 Use InnerException so if it fails again, it shows the exact SQL error instead of a generic message
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}