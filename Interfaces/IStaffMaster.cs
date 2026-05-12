using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IStaffMaster
    {
        // ⭐ SAVE
        Task<ResponseResult> SaveStaff(StaffMaster staff);

        // ⭐ LIST
        Task<ResponseResult> ListStaff();

        // ⭐ DETAIL BY ID
        Task<ResponseResult> DetailStaff(int id);

        // ⭐ UPDATE
        Task<ResponseResult> UpdateStaff(StaffMaster staff);

        // ⭐ DELETE
        Task<ResponseResult> DeleteStaff(int id);


        public Task<ResponseResult> Login(string email, string password);
        public Task<ResponseResult> ChangePassword(int staffId, string oldPassword, string newPassword);
        public Task<ResponseResult> ForgotPassword(string email);
    }
}
