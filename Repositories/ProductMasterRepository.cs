using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class ProductMasterRepository : IProductMaster
    {
        private readonly ApplicationDBContext _dbContext;

        public ProductMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE PRODUCT
        public async Task<ResponseResult> SaveProduct(ProductMaster product)
        {
            try
            {
                // 🔹 Trim values
                product.Code = product.Code?.Trim();
                product.Name = product.Name?.Trim();
                product.HSN = product.HSN?.Trim();
                product.Unit = product.Unit?.Trim();

                // 🔹 Required validations
                if (string.IsNullOrWhiteSpace(product.Name))
                    return new ResponseResult("Fail", "Product name is required");

                if (string.IsNullOrWhiteSpace(product.Unit))
                    return new ResponseResult("Fail", "Unit is required");

                if (string.IsNullOrWhiteSpace(product.HSN))
                    return new ResponseResult("Fail", "HSN is required");

                // 🔹 Length validations
                if (product.Name.Length > 100)
                    return new ResponseResult("Fail", "Product name cannot exceed 100 characters");

                if (product.Unit.Length > 10)
                    return new ResponseResult("Fail", "Unit cannot exceed 10 characters");

                // 🔹 HSN numeric validation
                if (!product.HSN.All(char.IsDigit))
                    return new ResponseResult("Fail", "HSN must be numeric");

                // 🔹 HSN length check
                if (product.HSN.Length != 4 &&
                    product.HSN.Length != 6 &&
                    product.HSN.Length != 8)
                    return new ResponseResult("Fail", "HSN must be 4, 6 or 8 digits");

                // 🔹 Category validation
                if (product.ProductCategoryId <= 0)
                    return new ResponseResult("Fail", "Category is required");

                var categoryExists = await _dbContext.ProductCategories
                    .AnyAsync(x => x.Id == product.ProductCategoryId);

                if (!categoryExists)
                    return new ResponseResult("Fail", "Invalid category");

                // 🔹 Price validation
                if (product.Price <= 0)
                    return new ResponseResult("Fail", "Selling price must be greater than 0");

                if (product.CostPrice <= 0)
                    return new ResponseResult("Fail", "Cost price must be greater than 0");

                if (product.Price < product.CostPrice)
                    return new ResponseResult("Fail",
                        "Selling price cannot be less than cost price");

                // 🔹 Gst validation
                if (product.Gst < 0 || product.Gst > 100)
                    return new ResponseResult("Fail",
                        "Gst must be between 0 and 100");

                // 🔹 Duplicate Name
                if (await _dbContext.ProductMasters
                    .AnyAsync(x => x.Name.ToLower() == product.Name.ToLower()))
                    return new ResponseResult("Fail",
                        "Product name already exists");

                // 🔹 Duplicate Code
                if (!string.IsNullOrEmpty(product.Code))
                {
                    if (await _dbContext.ProductMasters
                        .AnyAsync(x => x.Code.ToLower() == product.Code.ToLower()))
                        return new ResponseResult("Fail",
                            "Product code already exists");
                }
                // AUTO CALCULATE Gst AMOUNT

                decimal GstAmount = (product.Price * product.Gst) / 100;

                // FINAL PRICE WITH Gst

                decimal finalPrice = product.Price + GstAmount;
                // 🔹 Auto generate Product Code
                var lastProduct = await _dbContext.ProductMasters
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                int nextNumber = 1;

                if (lastProduct != null && !string.IsNullOrEmpty(lastProduct.Code))
                {
                    var lastCode = lastProduct.Code.Replace("PRD-", "");
                    int.TryParse(lastCode, out nextNumber);
                    nextNumber++;
                }

                product.Code = $"PRD-{nextNumber:D4}";

                _dbContext.ProductMasters.Add(product);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Product saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }


        // ⭐ LIST PRODUCTS
        // ⭐ LIST PRODUCTS (WITH LIVE STOCK)
        public async Task<ResponseResult> ListProduct()
        {
            try
            {
                var data = await _dbContext.ProductMasters
                    .Include(p => p.ProductCategory)
                    .Select(p => new
                    {
                        p.Id,
                        p.Code,
                        p.Name,
                        CategoryId = p.ProductCategoryId,
                        CategoryName = p.ProductCategory!.CategoryName,
                        p.Unit,
                        p.Price,
                        p.CostPrice,
                        p.Gst,
                        p.HSN,

                        // 🔥 STOCK CALCULATION
                        AvailableQty =

                            // TOTAL INWARD
                            (_dbContext.InwardStocks
                                .Where(i => i.PurchaseItem.ProductMasterId == p.Id)
                                .Sum(i => (decimal?)i.Qty) ?? 0)

                            -

                            // TOTAL USED
                            (_dbContext.StockUseds
                                .Where(s => s.InwardStock.PurchaseItem.ProductMasterId == p.Id)
                                .Sum(s => (decimal?)s.Qty) ?? 0)
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }


        // ⭐ DETAIL PRODUCT
        public async Task<ResponseResult> DetailProduct(int id)
        {
            try
            {
                var data = await _dbContext.ProductMasters
                    .Include(p => p.ProductCategory)
                    .Where(p => p.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        o.Code,
                        o.Name,
                        o.Unit,
                        o.Price,
                        o.CostPrice,
                        o.Gst,
                        o.HSN,
                        CategoryName = o.ProductCategory!.CategoryName
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "Product not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }


        // ⭐ UPDATE PRODUCT
        public async Task<ResponseResult> UpdateProduct(ProductMaster product)
        {
            try
            {
                var existing = await _dbContext.ProductMasters
                    .FirstOrDefaultAsync(x => x.Id == product.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Product not found");

                product.Name = product.Name?.Trim();
                product.HSN = product.HSN?.Trim();
                product.Unit = product.Unit?.Trim();

                if (string.IsNullOrWhiteSpace(product.Name))
                    return new ResponseResult("Fail", "Product name is required");

                if (string.IsNullOrWhiteSpace(product.Unit))
                    return new ResponseResult("Fail", "Unit is required");

                if (string.IsNullOrWhiteSpace(product.HSN))
                    return new ResponseResult("Fail", "HSN is required");

                if (!product.HSN.All(char.IsDigit))
                    return new ResponseResult("Fail", "HSN must be numeric");

                if (product.ProductCategoryId <= 0)
                    return new ResponseResult("Fail", "Category is required");

                var categoryExists = await _dbContext.ProductCategories
                    .AnyAsync(x => x.Id == product.ProductCategoryId);

                if (!categoryExists)
                    return new ResponseResult("Fail", "Invalid category");

                if (product.Price <= 0)
                    return new ResponseResult("Fail", "Selling price must be greater than 0");

                if (product.CostPrice <= 0)
                    return new ResponseResult("Fail", "Cost price must be greater than 0");

                if (product.Price < product.CostPrice)
                    return new ResponseResult("Fail",
                        "Selling price cannot be less than cost price");

                if (product.Gst < 0 || product.Gst > 100)
                    return new ResponseResult("Fail",
                        "Gst must be between 0 and 100");

                // Duplicate Name check
                if (await _dbContext.ProductMasters
                    .AnyAsync(x => x.Name.ToLower() == product.Name.ToLower()
                                && x.Id != product.Id))
                    return new ResponseResult("Fail",
                        "Product name already exists");

                existing.Name = product.Name;
                existing.ProductCategoryId = product.ProductCategoryId;
                existing.Unit = product.Unit;
                existing.Price = product.Price;
                existing.CostPrice = product.CostPrice;
                existing.Gst = product.Gst;
                existing.HSN = product.HSN;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Product updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message);
            }
        }


        // ⭐ DELETE PRODUCT
        public async Task<ResponseResult> DeleteProduct(int id)
        {
            try
            {
                var existing = await _dbContext.ProductMasters.FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Product not found");

                _dbContext.ProductMasters.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Product deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}