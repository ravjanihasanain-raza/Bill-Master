using Bill_Master.ApplicationContext;
using System.Text.RegularExpressions;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class SoftwareSettingsRepository : ISoftwareSettings
    {
        private readonly ApplicationDBContext _dbContext;

        public SoftwareSettingsRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE OR UPDATE SETTINGS
        public async Task<ResponseResult> SaveSettings(SoftwareSettings settings)
        {
            try
            {
                var existingRecord = await _dbContext.SoftwareSettings.FirstOrDefaultAsync();

                // 🔴 Format Validations (India Billing Standards)

                // Contact Number → 10 digits
                if (!Regex.IsMatch(settings.ContactNo, @"^[0-9]{10}$"))
                    return new ResponseResult("Fail", "Invalid contact number");

                // Email format
                if (!Regex.IsMatch(settings.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new ResponseResult("Fail", "Invalid email address");

                // PAN → ABCDE1234F
                if (!Regex.IsMatch(settings.PAN, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                    return new ResponseResult("Fail", "Invalid PAN number");

                // GstIN → 24ABCDE1234F1Z5
                if (!Regex.IsMatch(settings.GstIN,
                    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[A-Z0-9]{1}Z[A-Z0-9]{1}$"))
                    return new ResponseResult("Fail", "Invalid GstIN");

                // IFSC → SBIN0001234
                if (!Regex.IsMatch(settings.BankIFSC, @"^[A-Z]{4}0[A-Z0-9]{6}$"))
                    return new ResponseResult("Fail", "Invalid IFSC code");


                // 🔴 Duplicate Checks (excluding current record)
                if (string.IsNullOrWhiteSpace(settings.BusinessName))
                    return new ResponseResult("Fail", "Business name required");

                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.ContactNo == settings.ContactNo && x.Id != settings.Id))
                    return new ResponseResult("Fail", "Contact number already exists");


                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.Email.ToLower() == settings.Email.ToLower() && x.Id != settings.Id))
                    return new ResponseResult("Fail", "Email already exists");

                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.GstIN == settings.GstIN && x.Id != settings.Id))
                    return new ResponseResult("Fail", "GstIN already exists");

                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.PAN == settings.PAN && x.Id != settings.Id))
                    return new ResponseResult("Fail", "PAN already exists");

                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.AccountNumber == settings.AccountNumber && x.Id != settings.Id))
                    return new ResponseResult("Fail", "Account number already exists");
                if (await _dbContext.SoftwareSettings
                    .AnyAsync(x => x.BankIFSC == settings.BankIFSC && x.Id != settings.Id))
                    return new ResponseResult("Fail", "IFSC already exists");


                // ⭐ Insert or Update Logic

                if (existingRecord == null)
                {
                    _dbContext.SoftwareSettings.Add(settings);
                    await _dbContext.SaveChangesAsync();

                    return new ResponseResult("OK", "Settings saved successfully");
                }
                else
                {
                    existingRecord.BusinessName = settings.BusinessName;
                    existingRecord.AddressLine1 = settings.AddressLine1;
                    existingRecord.AddressLine2 = settings.AddressLine2;
                    existingRecord.AddressLine3 = settings.AddressLine3;
                    existingRecord.ContactNo = settings.ContactNo;
                    existingRecord.Email = settings.Email;
                    existingRecord.GstIN = settings.GstIN;
                    existingRecord.PAN = settings.PAN;
                    existingRecord.BankName = settings.BankName;
                    existingRecord.AccountHolderName = settings.AccountHolderName;
                    existingRecord.AccountNumber = settings.AccountNumber;
                    existingRecord.BankIFSC = settings.BankIFSC;
                    existingRecord.SignatureURL = settings.SignatureURL;
                    existingRecord.LogoURL = settings.LogoURL;

                    await _dbContext.SaveChangesAsync();

                    return new ResponseResult("OK", "Settings updated successfully");
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }



        // ⭐ GET SETTINGS
        public async Task<ResponseResult> GetSettings()
        {
            try
            {
                var data = await _dbContext.SoftwareSettings.FirstOrDefaultAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}
