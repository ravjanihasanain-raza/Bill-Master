using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IInvoiceMaster
    {
        Task<ResponseResult> SaveFullInvoice(InvoiceWithItemsDto data);

        Task<ResponseResult> ListInvoice();

        Task<ResponseResult> DetailInvoice(int id);

        Task<ResponseResult> UpdateInvoice(InvoiceWithItemsDto data);

        Task<ResponseResult> DeleteInvoice(int id);
    }
}