using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IExpenseCategory
    {
        Task<ResponseResult> SaveExpenseCategory(ExpenseCategory model);

        Task<ResponseResult> ListExpenseCategory();

        Task<ResponseResult> DetailExpenseCategory(int id);

        Task<ResponseResult> UpdateExpenseCategory(ExpenseCategory model);

        Task<ResponseResult> DeleteExpenseCategory(int id);
    }
}