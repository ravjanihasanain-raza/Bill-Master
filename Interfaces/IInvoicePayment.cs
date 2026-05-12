using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IInvoicePayment
    {
        Task<ResponseResult> SavePayment(InvoicePayment payment);

        Task<ResponseResult> ListPayment();

        Task<ResponseResult> DetailPayment(int id);

        Task<ResponseResult> ListByInvoice(int invoiceId);

        Task<ResponseResult> UpdatePayment(InvoicePayment payment);

        Task<ResponseResult> DeletePayment(int id);
    }
}