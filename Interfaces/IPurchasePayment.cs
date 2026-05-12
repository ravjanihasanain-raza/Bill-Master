using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IPurchasePayment
    {
        Task<ResponseResult> SavePayment(PurchasePayment payment);

        Task<ResponseResult> ListPayment();

        Task<ResponseResult> DetailPayment(int id);

        Task<ResponseResult> UpdatePayment(PurchasePayment payment);

        Task<ResponseResult> DeletePayment(int id);
    }
}