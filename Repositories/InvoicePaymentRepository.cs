using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class InvoicePaymentRepository : IInvoicePayment
    {
        private readonly ApplicationDBContext _dbContext;

        public InvoicePaymentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE PAYMENT
        public async Task<ResponseResult> SavePayment(InvoicePayment payment)
        {
            try
            {
                // 🔴 REQUIRED CHECKS

                if (payment.Amount <= 0)
                    return new ResponseResult("Fail", "Amount must be greater than 0");

                if (payment.PaymentDate == default)
                    return new ResponseResult("Fail", "Payment date is required");

                if (payment.PaymentDate > DateTime.Today)
                    return new ResponseResult("Fail",
                        "Payment date cannot be in the future");

                // 🔴 FK VALIDATIONS

                var invoice = await _dbContext.InvoiceMasters
                    .FirstOrDefaultAsync(x => x.Id == payment.InvoiceMasterId);

                if (invoice == null)
                    return new ResponseResult("Fail", "Invalid Invoice");

                if (!await _dbContext.StaffMasters
                        .AnyAsync(x => x.Id == payment.StaffMasterId))
                    return new ResponseResult("Fail", "Invalid Staff");

                // 🔴 DATE LOGIC

                //if (payment.PaymentDate < invoice.InvoiceDate)
                //    return new ResponseResult("Fail",
                //        "Payment date cannot be before invoice date");

                // 🔴 OVER-PAYMENT PREVENTION

                var totalPaid = await _dbContext.InvoicePayments
                    .Where(x => x.InvoiceMasterId == payment.InvoiceMasterId)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;

                if (totalPaid + payment.Amount > invoice.Total)
                    return new ResponseResult("Fail",
                        "Payment exceeds invoice total");

                payment.CreatedAt = DateTime.Now;

                _dbContext.InvoicePayments.Add(payment);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Invoice payment saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST PAYMENTS
        public async Task<ResponseResult> ListPayment()
        {
            try
            {
                var data = await _dbContext.InvoicePayments
                    .Include(p => p.InvoiceMaster)
                    .Include(p => p.StaffMaster)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        p.PaymentDate,
                        p.ReferenceNo,
                        p.InvoiceMasterId,
                        InvoiceNo = p.InvoiceMaster!.InvoiceNo,
                        StaffName = p.StaffMaster!.FullName
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL BY ID
        public async Task<ResponseResult> DetailPayment(int id)
        {
            try
            {
                var data = await _dbContext.InvoicePayments
                    .Include(p => p.InvoiceMaster)
                    .Include(p => p.StaffMaster)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (data == null)
                    return new ResponseResult("Fail", "Payment not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
        
        public async Task<ResponseResult> ListByInvoice(int invoiceId)
        {
            try
            {
                var data = await _dbContext.InvoicePayments
                    .Where(p => p.InvoiceMasterId == invoiceId)
                    .Include(p => p.StaffMaster)
                    .OrderByDescending(p => p.PaymentDate)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        p.PaymentDate,
                        p.ReferenceNo,
                        StaffName = p.StaffMaster!.FullName
                    })
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE PAYMENT
        public async Task<ResponseResult> UpdatePayment(InvoicePayment payment)
        {
            try
            {
                var existing = await _dbContext.InvoicePayments
                    .FindAsync(payment.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Payment not found");

                if (payment.Amount <= 0)
                    return new ResponseResult("Fail", "Amount must be greater than 0");

                // 🔴 FK VALIDATION AGAIN

                var invoice = await _dbContext.InvoiceMasters
                    .FirstOrDefaultAsync(x => x.Id == payment.InvoiceMasterId);

                if (invoice == null)
                    return new ResponseResult("Fail", "Invalid Invoice");

                if (!await _dbContext.StaffMasters
                        .AnyAsync(x => x.Id == payment.StaffMasterId))
                    return new ResponseResult("Fail", "Invalid Staff");

                // 🔴 DATE LOGIC

                if (payment.PaymentDate < invoice.InvoiceDate)
                    return new ResponseResult("Fail",
                        "Payment date cannot be before invoice date");

                // 🔴 OVER-PAYMENT CHECK (excluding current record)

                var totalPaid = await _dbContext.InvoicePayments
                    .Where(x => x.InvoiceMasterId == payment.InvoiceMasterId
                             && x.Id != payment.Id)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0;

                if (totalPaid + payment.Amount > invoice.Total)
                    return new ResponseResult("Fail",
                        "Payment exceeds invoice total");

                // 🔄 UPDATE

                existing.InvoiceMasterId = payment.InvoiceMasterId;
                existing.Amount = payment.Amount;
                existing.PaymentDate = payment.PaymentDate;
                existing.ReferenceNo = payment.ReferenceNo;
                existing.StaffMasterId = payment.StaffMasterId;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Payment updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DELETE PAYMENT
        public async Task<ResponseResult> DeletePayment(int id)
        {
            try
            {
                var existing = await _dbContext.InvoicePayments
                    .FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Payment not found");

                _dbContext.InvoicePayments.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Payment deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}