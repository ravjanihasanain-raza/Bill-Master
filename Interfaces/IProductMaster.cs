using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IProductMaster
    {
        // ⭐ SAVE PRODUCT
        Task<ResponseResult> SaveProduct(ProductMaster product);

        // ⭐ LIST ALL PRODUCTS
        Task<ResponseResult> ListProduct();

        // ⭐ GET PRODUCT BY ID
        Task<ResponseResult> DetailProduct(int id);

        // ⭐ UPDATE PRODUCT
        Task<ResponseResult> UpdateProduct(ProductMaster product);

        // ⭐ DELETE PRODUCT
        Task<ResponseResult> DeleteProduct(int id);
    }
}
