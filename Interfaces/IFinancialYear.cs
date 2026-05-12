using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IFinancialYear
    {
        Task<ResponseResult> SaveFinancialYear(FinancialYear year);

        Task<ResponseResult> ListFinancialYear();

        Task<ResponseResult> DetailFinancialYear(int id);

        Task<ResponseResult> UpdateFinancialYear(FinancialYear year);

        Task<ResponseResult> DeleteFinancialYear(int id);
        Task<ResponseResult> SetActiveYear(int id);
        Task<ResponseResult> CloseYear(int id);
    }
}
