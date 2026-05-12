using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bill_Master.Repositories
{
    public class VendorRepository : IVendor
    {
        private readonly ApplicationDBContext _dbContext;

        public VendorRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE
        public async Task<ResponseResult> SaveVendor(Vendor vendor)
        {
            try
            {
                // 🔴 Email format
                //if (!Regex.IsMatch(vendor.Email,
                //    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                //    return new ResponseResult("Fail", "Invalid email format");

                // 🔴 Contact number (10 digits)
                //if (!Regex.IsMatch(vendor.ContactNo, @"^\d{10}$"))
                //    return new ResponseResult("Fail",
                //        "Contact number must be 10 digits");

                // 🔴 GstIN format
                //if (!Regex.IsMatch(vendor.GstIN,
                //    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$"))
                //    return new ResponseResult("Fail", "Invalid GstIN");

                //if (!Regex.IsMatch(vendor.AccountNumber,
                //    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$"))
                //    return new ResponseResult("Fail", "Invalid AccountNumber");

                // 🔴 PAN format
                //if (!Regex.IsMatch(vendor.PAN,
                //    @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                //    return new ResponseResult("Fail", "Invalid PAN");

                // 🔴 IFSC format
                //if (!Regex.IsMatch(vendor.IFSC,
                //    @"^[A-Z]{4}0[A-Z0-9]{6}$"))
                //    return new ResponseResult("Fail", "Invalid IFSC code");

                // 🔴 Duplicate Contact
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.ContactNo == vendor.ContactNo))
                //    return new ResponseResult("Fail",
                //        "Contact number already exists");

                // 🔴 Duplicate Email
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.Email.ToLower() == vendor.Email.ToLower()))
                //    return new ResponseResult("Fail",
                //        "Email already exists");

                // 🔴 Duplicate GstIN
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.GstIN == vendor.GstIN))
                //    return new ResponseResult("Fail",
                //        "GstIN already exists");

                // 🔴 Duplicate PAN
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.PAN == vendor.PAN))
                //    return new ResponseResult("Fail",
                //        "PAN already exists");

                // 🔴 Duplicate IFSC
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.IFSC == vendor.IFSC))
                //    return new ResponseResult("Fail",
                //        "IFSC already exists");

                // 🔴 Duplicate Account Number
        //        if (await _dbContext.Vendors
        //            .AnyAsync(x => x.AccountNumber == vendor.AccountNumber))
        //            return new ResponseResult("Fail",
        //                "Account number already exists");

               vendor.CreatedAt = DateTime.Now;
                       _dbContext.Vendors.Add(vendor);
                await _dbContext.SaveChangesAsync();
                       return new ResponseResult("OK", "Vendor saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
          }
        }


        // ⭐ LIST
        public async Task<ResponseResult> ListVendor()
        {
            try
            {
                var data = await _dbContext.Vendors.ToListAsync();
                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL BY ID
        public async Task<ResponseResult> DetailVendor(int id)
        {
            try
            {
                var data = await _dbContext.Vendors.FindAsync(id);

                if (data == null)
                    return new ResponseResult("Fail", "Vendor not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE
        public async Task<ResponseResult> UpdateVendor(Vendor vendor)
        {
            try
            {
                var existing = await _dbContext.Vendors.FindAsync(vendor.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Vendor not found");

                // 🔴 Required fields
                //if (string.IsNullOrWhiteSpace(vendor.BusinessName))
                //    return new ResponseResult("Fail", "Business name is required");

                //if (string.IsNullOrWhiteSpace(vendor.Address))
                //    return new ResponseResult("Fail", "Address is required");

                //if (string.IsNullOrWhiteSpace(vendor.StateCode))
                //    return new ResponseResult("Fail", "State code is required");

                //if (string.IsNullOrWhiteSpace(vendor.BankName))
                //    return new ResponseResult("Fail", "Bank name is required");

                //if (string.IsNullOrWhiteSpace(vendor.AccountHolder))
                //    return new ResponseResult("Fail", "Account holder is required");

                // 🔴 Email format
                //if (!Regex.IsMatch(vendor.Email,
                //    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                //    return new ResponseResult("Fail", "Invalid email format");

                // 🔴 Contact number (10 digits)
                //if (!Regex.IsMatch(vendor.ContactNo, @"^\d{10}$"))
                //    return new ResponseResult("Fail",
                //        "Contact number must be 10 digits");

                // 🔴 GstIN format
                //if (!Regex.IsMatch(vendor.GstIN,
                //    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$"))
                //    return new ResponseResult("Fail", "Invalid GstIN");

                // 🔴 PAN format
                //if (!Regex.IsMatch(vendor.PAN,
                //    @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
                //    return new ResponseResult("Fail", "Invalid PAN");

                // 🔴 IFSC format
                //if (!Regex.IsMatch(vendor.IFSC,
                //    @"^[A-Z]{4}0[A-Z0-9]{6}$"))
                //    return new ResponseResult("Fail", "Invalid IFSC code");

                // 🔴 Duplicate Contact (exclude current)
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.ContactNo == vendor.ContactNo
                //                && x.Id != vendor.Id))
                //    return new ResponseResult("Fail",
                //        "Contact number already exists");

                // 🔴 Duplicate Email
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.Email.ToLower() == vendor.Email.ToLower()
                //                && x.Id != vendor.Id))
                //    return new ResponseResult("Fail",
                //        "Email already exists");

                // 🔴 Duplicate GstIN
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.GstIN == vendor.GstIN
                //                && x.Id != vendor.Id))
                //    return new ResponseResult("Fail",
                //        "GstIN already exists");

                // 🔴 Duplicate PAN
                //if (await _dbContext.Vendors
                //    .AnyAsync(x => x.PAN == vendor.PAN
                //                && x.Id != vendor.Id))
                //    return new ResponseResult("Fail",
                //        "PAN already exists");

                // 🔴 Duplicate Account Number
                if (await _dbContext.Vendors
                    .AnyAsync(x => x.AccountNumber == vendor.AccountNumber
                                && x.Id != vendor.Id))
                    return new ResponseResult("Fail",
                        "Account number already exists");

                // 🔄 Update fields
                existing.BusinessName = vendor.BusinessName;
                existing.ContactPerson = vendor.ContactPerson;
                existing.GstIN = vendor.GstIN;
                existing.PAN = vendor.PAN;
                existing.Address = vendor.Address;
                existing.StateCode = vendor.StateCode;
                existing.BankName = vendor.BankName;
                existing.AccountHolder = vendor.AccountHolder;
                existing.IFSC = vendor.IFSC;
                existing.AccountNumber = vendor.AccountNumber;
                existing.Email = vendor.Email;
                existing.ContactNo = vendor.ContactNo;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Vendor updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }


        // ⭐ DELETE (Hard Delete)
        public async Task<ResponseResult> DeleteVendor(int id)
        {
            try
            {
                var existing = await _dbContext.Vendors.FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Vendor not found");

                _dbContext.Vendors.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Vendor deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}
