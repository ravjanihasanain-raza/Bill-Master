using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IInwardStock
    {
        Task<ResponseResult> SaveInward(InwardStock inward);

        Task<ResponseResult> ListInward();

        Task<ResponseResult> DetailInward(int id);

        Task<ResponseResult> UpdateInward(InwardStock inward);

        Task<ResponseResult> DeleteInward(int id);
    }
}