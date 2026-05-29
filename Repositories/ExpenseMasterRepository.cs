using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class ExpenseMasterRepository : IExpenseMaster
    {
        private readonly ApplicationDBContext _context;

        public ExpenseMasterRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ResponseResult> SaveExpense(ExpenseMaster model)
        {
            try
            {
                if (model.Amount <= 0)
                {
                    return new ResponseResult("Fail", "Amount must be greater than zero");
                }

                if (string.IsNullOrWhiteSpace(model.Description))
                {
                    return new ResponseResult("Fail", "Description is required");
                }

                bool categoryExists = await _context.ExpenseCategory
                    .AnyAsync(x => x.Id == model.ExpenseCategoryId);

                if (!categoryExists)
                {
                    return new ResponseResult("Fail", "Invalid Expense Category");
                }

                await _context.ExpenseMaster.AddAsync(model);

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Expense Saved");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ListExpense()
        {
            try
            {
                var data = await (
                    from e in _context.ExpenseMaster
                    join c in _context.ExpenseCategory
                    on e.ExpenseCategoryId equals c.Id
                    orderby e.ExpenseDate descending
                    select new ExpenseListDto
                    {
                        Id = e.Id,
                        ExpenseDate = e.ExpenseDate,
                        CategoryName = c.CategoryName,
                        Amount = e.Amount,
                        Description = e.Description,
                        PaymentMode = e.PaymentMode,
                        PaidTo = e.PaidTo,

                        IsPaid = e.IsPaid,
                        PaidDate = e.PaidDate,
                        PaidBy = e.PaidBy
                    }
                ).ToListAsync();

                return new ResponseResult("OK", "Data Found", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DetailExpense(int id)
        {
            try
            {
                var data = await _context.ExpenseMaster
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Expense not found");
                }

                return new ResponseResult("OK", "Data Found", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> UpdateExpense(ExpenseMaster model)
        {
            try
            {
                var data = await _context.ExpenseMaster
                    .FirstOrDefaultAsync(x => x.Id == model.Id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Expense not found");
                }

                if (model.Amount <= 0)
                {
                    return new ResponseResult("Fail", "Amount must be greater than zero");
                }

                data.ExpenseDate = model.ExpenseDate;
                data.Amount = model.Amount;
                data.ExpenseCategoryId = model.ExpenseCategoryId;
                data.Description = model.Description;
                data.PaymentMode = model.PaymentMode;
                data.PaidTo = model.PaidTo;
                data.ReferenceNo = model.ReferenceNo;
                data.IsApproved = model.IsApproved;
                data.AttachmentURL = model.AttachmentURL;

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Expense Updated");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DeleteExpense(int id)
        {
            try
            {
                var data = await _context.ExpenseMaster
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Expense not found");
                }

                _context.ExpenseMaster.Remove(data);

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Expense Deleted");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> MarkPaid(int id)
        {
            try
            {
                var data = await _context.ExpenseMaster
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Expense not found");
                }

                if (data.IsPaid)
                {
                    return new ResponseResult("Fail", "Expense already paid");
                }

                data.IsPaid = true;
                data.PaidDate = DateTime.Now;
                data.PaidBy = "Admin";

                await _context.SaveChangesAsync();

                return new ResponseResult("OK", "Expense marked as paid");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}