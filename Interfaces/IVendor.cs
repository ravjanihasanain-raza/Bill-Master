using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IVendor
    {
        // ⭐ SAVE
        Task<ResponseResult> SaveVendor(Vendor vendor);

        // ⭐ LIST
        Task<ResponseResult> ListVendor();

        // ⭐ DETAIL BY ID
        Task<ResponseResult> DetailVendor(int id);

        // ⭐ UPDATE
        Task<ResponseResult> UpdateVendor(Vendor vendor);

        // ⭐ DELETE
        Task<ResponseResult> DeleteVendor(int id);
    }
}
