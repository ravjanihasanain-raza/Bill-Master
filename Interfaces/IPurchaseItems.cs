using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IPurchaseItems
    {
        Task<ResponseResult> SaveItem(PurchaseItems item);

        Task<ResponseResult> ListItem();

        Task<ResponseResult> DetailItem(int id);

        Task<ResponseResult> UpdateItem(PurchaseItems item);

        Task<ResponseResult> DeleteItem(int id);
    }
}