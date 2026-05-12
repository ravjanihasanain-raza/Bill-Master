using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IClientMaster
    {
        // ⭐ SAVE CLIENT
        Task<ResponseResult> SaveClient(ClientMaster client);

        // ⭐ LIST ALL CLIENTS
        Task<ResponseResult> ListClient();

        // ⭐ GET CLIENT BY ID
        Task<ResponseResult> DetailClient(int id);

        // ⭐ UPDATE CLIENT
        Task<ResponseResult> UpdateClient(ClientMaster client);

        // ⭐ DELETE CLIENT
        Task<ResponseResult> DeleteClient(int id);
    }
}
