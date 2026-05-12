using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IAdminRepository
    {
        public Task<ResponseResult> SaveAdmin(Admin admin );

        public Task<ResponseResult> ListAdmin();

        public Task<ResponseResult> DeleteAdmin(int Id);

        public Task<ResponseResult> DetailAdmin(int Id);

        public Task<ResponseResult> UpdateAdmin(Admin admin );

        public Task<ResponseResult> Login(string email, string password);

        public Task<ResponseResult> ChangePassword(int adminId, string oldPassword, string newPassword);

        public Task<ResponseResult> ForgotPassword(string email);
    }
}
