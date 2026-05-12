using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IPurchaseMaster
    {
        Task<ResponseResult> SavePurchase(PurchaseMaster purchase);

        Task<ResponseResult> ListPurchase();

        Task<ResponseResult> DetailPurchase(int id);

        Task<ResponseResult> UpdatePurchase(PurchaseMaster purchase);

        Task<ResponseResult> DeletePurchase(int id);
    }
}