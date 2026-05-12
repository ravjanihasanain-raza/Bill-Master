using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class PurchasePaymentRepository : IPurchasePayment
    {
        private readonly ApplicationDBContext _dbContext;

        public PurchasePaymentRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE PAYMENT
        public async Task<ResponseResult> SavePayment(PurchasePayment payment)
        {
            try
            {
                // 🔴 BASIC VALIDATION
                if (payment.Amount <= 0)
                    return new ResponseResult("Fail", "Amount must be greater than 0");

                if (payment.PaymentDate == default)
                    return new ResponseResult("Fail", "Payment date is required");

                // 🔴 PURCHASE FETCH WITH PAYMENTS
                var purchase = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchasePayments)
                    .FirstOrDefaultAsync(x => x.Id == payment.PurchaseMasterId);

                if (purchase == null)
                    return new ResponseResult("Fail", "Invalid Purchase");

                // 🔴 STAFF VALIDATION
                var staffExists = await _dbContext.StaffMasters
                    .AnyAsync(x => x.Id == payment.StaffMasterId);

                if (!staffExists)
                    return new ResponseResult("Fail", "Invalid Staff");

                // 🔴 DUPLICATE REF CHECK
                if (!string.IsNullOrWhiteSpace(payment.ReferenceNo))
                {
                    var duplicate = await _dbContext.PurchasePayments
                        .AnyAsync(x =>
                            x.PurchaseMasterId == payment.PurchaseMasterId &&
                            x.ReferenceNo == payment.ReferenceNo);

                    if (duplicate)
                        return new ResponseResult("Fail", "Duplicate reference number");
                }

                // 🔥 BUSINESS LOGIC (CRITICAL)
                var paid = purchase.PurchasePayments.Sum(x => x.Amount);
                var remaining = purchase.Total - paid;

                if (payment.Amount > remaining)
                    return new ResponseResult("Fail",
                        $"Only {remaining} amount is pending");

                // ✅ SAVE
                payment.CreatedAt = DateTime.Now;

                await _dbContext.PurchasePayments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Payment saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST PAYMENTS (FK INCLUDE)
        public async Task<ResponseResult> ListPayment()
        {
            try
            {
                var data = await _dbContext.PurchasePayments
                    .Include(p => p.PurchaseMaster)
                    .Include(p => p.StaffMaster)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        p.PaymentDate,
                        p.ReferenceNo,
                        PurchaseBillNo = p.PurchaseMaster!.BillNo,
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
                var data = await _dbContext.PurchasePayments
                    .Include(p => p.PurchaseMaster)
                    .Include(p => p.StaffMaster)
                    .Where(p => p.Id == id)
                    .Select(p => new
                    {
                        p.Id,
                        p.Amount,
                        p.PaymentDate,
                        p.ReferenceNo,
                        p.Remarks,

                        PurchaseBillNo = p.PurchaseMaster != null
                            ? p.PurchaseMaster.BillNo
                            : null,

                        StaffName = p.StaffMaster != null
                            ? p.StaffMaster.FullName
                            : null
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return new ResponseResult("Fail", "Payment not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
        // ⭐ UPDATE PAYMENT
        public async Task<ResponseResult> UpdatePayment(PurchasePayment payment)
        {
            try
            {
                var existing = await _dbContext.PurchasePayments
                    .FindAsync(payment.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Payment not found");

                if (payment.Amount <= 0)
                    return new ResponseResult("Fail", "Amount must be greater than 0");

                var purchase = await _dbContext.PurchaseMasters
                    .Include(p => p.PurchasePayments)
                    .FirstOrDefaultAsync(x => x.Id == payment.PurchaseMasterId);

                if (purchase == null)
                    return new ResponseResult("Fail", "Invalid Purchase");

                // 🔥 REMOVE CURRENT PAYMENT FROM CALCULATION
                var paid = purchase.PurchasePayments
                    .Where(x => x.Id != payment.Id)
                    .Sum(x => x.Amount);

                var remaining = purchase.Total - paid;

                if (payment.Amount > remaining)
                    return new ResponseResult("Fail",
                        $"Only {remaining} amount is pending");

                // ✅ UPDATE
                existing.PurchaseMasterId = payment.PurchaseMasterId;
                existing.Amount = payment.Amount;
                existing.PaymentDate = payment.PaymentDate;
                existing.ReferenceNo = payment.ReferenceNo;
                existing.StaffMasterId = payment.StaffMasterId;
                existing.Remarks = payment.Remarks;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Payment updated successfully");
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
                var existing = await _dbContext.PurchasePayments
                    .FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Payment not found");

                _dbContext.PurchasePayments.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Payment deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}