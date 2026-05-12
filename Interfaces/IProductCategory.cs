using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IProductCategory
    {
        // ⭐ SAVE CATEGORY
        Task<ResponseResult> SaveCategory(ProductCategory category);

        // ⭐ LIST CATEGORY
        Task<ResponseResult> ListCategory();

        // ⭐ DETAIL BY ID
        Task<ResponseResult> DetailCategory(int id);

        // ⭐ UPDATE CATEGORY
        Task<ResponseResult> UpdateCategory(ProductCategory category);

        // ⭐ DELETE CATEGORY (Soft Delete)
        Task<ResponseResult> DeleteCategory(int id);
    }
}
