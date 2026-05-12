using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IStockRepository
    {
        Task<ResponseResult> GetLowStockProducts();
        Task<ResponseResult> GetStockHistory(int productId);
    }
}