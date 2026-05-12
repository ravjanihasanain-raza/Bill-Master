using Bill_Master.Model;
using System.Threading.Tasks;

namespace Bill_Master.Interfaces
{
    public interface IOutward
    {
        Task<ResponseResult> SaveOutward(Outward outward);

        Task<ResponseResult> ListOutward();

        Task<ResponseResult> DetailOutward(int id);

        Task<ResponseResult> DeleteOutward(int id);

        Task<ResponseResult> UpdateOutward(Outward outward);
        Task<ResponseResult> AutoFromInvoice(InvoiceDto dto);
    }
}