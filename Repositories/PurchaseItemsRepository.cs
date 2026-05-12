using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class PurchaseItemsRepository : IPurchaseItems
    {
        private readonly ApplicationDBContext _dbContext;

        public PurchaseItemsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE ITEM (AUTO CALC)
        public async Task<ResponseResult> SaveItem(PurchaseItems item)
        {
            try
            {
                // 🔴 FK VALIDATION

                if (!await _dbContext.ProductMasters
                        .AnyAsync(x => x.Id == item.ProductMasterId))
                    return new ResponseResult("Fail", "Invalid Product");

                if (!await _dbContext.PurchaseMasters
                        .AnyAsync(x => x.Id == item.PurchaseMasterId))
                    return new ResponseResult("Fail", "Invalid Purchase");

                // 🔴 DUPLICATE CHECK

                var duplicateExists = await _dbContext.PurchaseItems
                    .AnyAsync(x =>
                        x.PurchaseMasterId == item.PurchaseMasterId &&
                        x.ProductMasterId == item.ProductMasterId);

                if (duplicateExists)
                    return new ResponseResult("Fail",
                        "This product is already added in this purchase");

                // 🔴 REQUIRED CHECKS

                if (item.Qty <= 0)
                    return new ResponseResult("Fail", "Qty must be > 0");

                if (item.Rate <= 0)
                    return new ResponseResult("Fail", "Rate must be > 0");

                // ⭐ AUTO CALCULATION

                item.Amount = item.Qty * item.Rate;

                var discount = item.Discount ?? 0;

                item.Total = item.Amount - discount;

                _dbContext.PurchaseItems.Add(item);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Item saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ⭐ LIST ITEMS
        public async Task<ResponseResult> ListItem()
        {
            try
            {
                var data = await _dbContext.PurchaseItems
                    .Include(i => i.ProductMaster)
                    .Include(i => i.PurchaseMaster)
                    .Select(i => new
                    {
                        i.Id,
                        Product = i.ProductMaster!.Name,
                        i.Qty,
                        i.Rate,
                        i.Amount,
                        i.Total,
                        PurchaseBill = i.PurchaseMaster!.BillNo
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
        public async Task<ResponseResult> DetailItem(int id)
        {
            try
            {
                var data = await _dbContext.PurchaseItems
                    .Include(i => i.ProductMaster)
                    .Include(i => i.PurchaseMaster)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (data == null)
                    return new ResponseResult("Fail", "Item not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE ITEM (AUTO AGAIN)
        public async Task<ResponseResult> UpdateItem(PurchaseItems item)
        {
            try
            {
                // 🔴 DUPLICATE CHECK (EXCLUDE CURRENT)
                var duplicateExists = await _dbContext.PurchaseItems
                    .AnyAsync(x =>
                        x.PurchaseMasterId == item.PurchaseMasterId &&
                        x.ProductMasterId == item.ProductMasterId &&
                        x.Id != item.Id);

                if (duplicateExists)
                    return new ResponseResult("Fail",
                        "This product is already added in this purchase");

                var existing = await _dbContext.PurchaseItems.FindAsync(item.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Item not found");

                if (item.Qty <= 0)
                    return new ResponseResult("Fail", "Qty must be > 0");

                if (item.Rate <= 0)
                    return new ResponseResult("Fail", "Rate must be > 0");

                // ⭐ AUTO CALCULATION AGAIN

                existing.ProductMasterId = item.ProductMasterId;
                existing.HSNCode = item.HSNCode;
                existing.Qty = item.Qty;
                existing.Rate = item.Rate;
                existing.Discount = item.Discount;

                existing.Amount = item.Qty * item.Rate;

                var discount = item.Discount ?? 0;

                existing.Total = existing.Amount - discount;

                existing.PurchaseMasterId = item.PurchaseMasterId;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Item updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ⭐ DELETE ITEM
        public async Task<ResponseResult> DeleteItem(int id)
        {
            try
            {
                var existing = await _dbContext.PurchaseItems.FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Item not found");

                // 🛡️ ERP SAFETY CHECK: Check if this item is already inwarded in inventory
                var hasInwardStock = await _dbContext.InwardStocks
                    .AnyAsync(i => i.PurchaseItemId == id);

                if (hasInwardStock)
                {
                    return new ResponseResult("Fail", "Cannot delete: This item has already been added to inventory (Inward Stock). Please delete the Inward Stock entry first.");
                }

                _dbContext.PurchaseItems.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Item deleted successfully");
            }
            catch (Exception ex)
            {
                // 🔥 Catch SQL Foreign Key constraint errors perfectly
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}