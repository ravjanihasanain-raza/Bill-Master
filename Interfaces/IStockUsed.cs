using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IStockUsed
    {
        Task<ResponseResult> SaveStockUsed(StockUsed stockUsed);

        Task<ResponseResult> ListStockUsed();

        Task<ResponseResult> DetailStockUsed(int id);

        Task<ResponseResult> UpdateStockUsed(StockUsed stockUsed);

        Task<ResponseResult> DeleteStockUsed(int id);
    }
}