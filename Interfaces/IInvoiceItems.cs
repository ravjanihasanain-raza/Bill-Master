using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IInvoiceItems
    {
        Task<ResponseResult> SaveInvoiceItem(InvoiceItems item);

        Task<ResponseResult> ListInvoiceItems();

        Task<ResponseResult> DetailInvoiceItem(int id);

        Task<ResponseResult> UpdateInvoiceItem(InvoiceItems item);

        Task<ResponseResult> DeleteInvoiceItem(int id);
    }
}