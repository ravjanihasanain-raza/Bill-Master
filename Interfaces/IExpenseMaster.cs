using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IExpenseMaster
    {
        Task<ResponseResult> SaveExpense(ExpenseMaster model);

        Task<ResponseResult> ListExpense();

        Task<ResponseResult> DetailExpense(int id);

        Task<ResponseResult> UpdateExpense(ExpenseMaster model);

        Task<ResponseResult> DeleteExpense(int id);
        Task<ResponseResult> MarkPaid(int id);
    }
}