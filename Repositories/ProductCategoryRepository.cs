using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class ProductCategoryRepository : IProductCategory
    {
        private readonly ApplicationDBContext _dbContext;

        public ProductCategoryRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE CATEGORY
        public async Task<ResponseResult> SaveCategory(ProductCategory category)
        {
            try
            {
                // 🔴 Duplicate Name Check (Case-Insensitive)
                var exists = await _dbContext.ProductCategories
                    .AnyAsync(x => x.CategoryName.ToLower() ==
                                   category.CategoryName.ToLower()
                                   && !x.IsDelete);

                if (exists)
                    return new ResponseResult("Fail",
                        "Category already exists");

                category.CreatedAt = DateTime.Now;
                category.IsDelete = false;

                _dbContext.ProductCategories.Add(category);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Category saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST CATEGORY (Only Active)
        public async Task<ResponseResult> ListCategory()
        {
            try
            {
                var data = await _dbContext.ProductCategories
                    .Where(x => !x.IsDelete)
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL BY ID
        public async Task<ResponseResult> DetailCategory(int id)
        {
            try
            {
                var data = await _dbContext.ProductCategories
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete);

                if (data == null)
                    return new ResponseResult("Fail",
                        "Category not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE CATEGORY
        public async Task<ResponseResult> UpdateCategory(ProductCategory category)
        {
            try
            {
                var existing = await _dbContext.ProductCategories
                    .FindAsync(category.Id);

                if (existing == null || existing.IsDelete)
                    return new ResponseResult("Fail",
                        "Category not found");

                // 🔴 Duplicate Name Check (excluding current)
                var exists = await _dbContext.ProductCategories
                    .AnyAsync(x => x.CategoryName.ToLower() ==
                                   category.CategoryName.ToLower()
                                   && x.Id != category.Id
                                   && !x.IsDelete);

                if (exists)
                    return new ResponseResult("Fail",
                        "Category already exists");

                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Category updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DELETE CATEGORY (Soft Delete)
        public async Task<ResponseResult> DeleteCategory(int id)
        {
            try
            {
                var existing = await _dbContext.ProductCategories
                    .FindAsync(id);

                if (existing == null || existing.IsDelete)
                    return new ResponseResult("Fail",
                        "Category not found");

                existing.IsDelete = true;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Category deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}
