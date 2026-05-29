using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategory
    {
        private readonly ApplicationDBContext _context;

        public ExpenseCategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseResult> SaveExpenseCategory(ExpenseCategory model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.CategoryName))
                {
                    return new ResponseResult("Fail", "Category Name is required");
                }

                bool exists = await _context.ExpenseCategory
                    .AnyAsync(x => x.CategoryName.ToLower() == model.CategoryName.ToLower());

                if (exists)
                {
                    return new ResponseResult("Fail", "Category already exists");
                }

                await _context.ExpenseCategory.AddAsync(model);
                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Expense Category Saved");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ListExpenseCategory()
        {
            try
            {
                var data = await _context.ExpenseCategory
                    .OrderBy(x => x.CategoryName)
                    .ToListAsync();

                return new ResponseResult("OK", "Data Found", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DetailExpenseCategory(int id)
        {
            try
            {
                var data = await _context.ExpenseCategory
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Category not found");
                }

                return new ResponseResult("OK", "Data Found", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> UpdateExpenseCategory(ExpenseCategory model)
        {
            try
            {
                var data = await _context.ExpenseCategory
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Category not found");
                }

                bool exists = await _context.ExpenseCategory
                    .AnyAsync(x =>
                        x.Id != model.Id &&
                        x.CategoryName.ToLower() == model.CategoryName.ToLower());

                if (exists)
                {
                    return new ResponseResult("Fail", "Category already exists");
                }

                data.CategoryName = model.CategoryName;
                data.Description = model.Description;
                data.IsActive = model.IsActive;

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Category Updated");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DeleteExpenseCategory(int id)
        {
            try
            {
                var data = await _context.ExpenseCategory
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Category not found");
                }

                _context.ExpenseCategory.Remove(data);

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Category Deleted");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}