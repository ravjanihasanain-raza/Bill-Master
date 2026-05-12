using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface ISoftwareSettings
    {
        // ⭐ SAVE OR UPDATE SETTINGS
        Task<ResponseResult> SaveSettings(SoftwareSettings settings);

        // ⭐ GET SETTINGS (Single Record)
        Task<ResponseResult> GetSettings();
    }
}
