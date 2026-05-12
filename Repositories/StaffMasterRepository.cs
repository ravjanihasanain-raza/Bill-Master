using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bill_Master.Repositories
{
    public class StaffMasterRepository : IStaffMaster
    {
        private readonly ApplicationDBContext _dbContext;

        public StaffMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ PASSWORD GENERATOR
        private string GeneratePassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // ⭐ SAVE STAFF (Updated with Strict Validation, Hashing & Email)
        public async Task<ResponseResult> SaveStaff(StaffMaster staff)
        {
            try
            {
                // 🔴 STRICT REQUIRED VALIDATIONS
                if (string.IsNullOrWhiteSpace(staff.FullName)) return new ResponseResult("Fail", "Full Name is required");
                if (string.IsNullOrWhiteSpace(staff.Email)) return new ResponseResult("Fail", "Email is required");
                if (string.IsNullOrWhiteSpace(staff.ContactNo)) return new ResponseResult("Fail", "Contact Number is required");
                if (string.IsNullOrWhiteSpace(staff.Role)) return new ResponseResult("Fail", "Role is required");

                // 🔴 Email Format Validation
                if (!Regex.IsMatch(staff.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new ResponseResult("Fail", "Invalid email format");

                // 🔴 Contact Number Validation (10 digits)
                if (!Regex.IsMatch(staff.ContactNo, @"^\d{10}$"))
                    return new ResponseResult("Fail", "Contact number must be 10 digits");

                // 🔴 Duplicate Checks
                if (await _dbContext.StaffMasters.AnyAsync(x => x.Email.ToLower() == staff.Email.ToLower()))
                    return new ResponseResult("Fail", "Email already exists");

                if (await _dbContext.StaffMasters.AnyAsync(x => x.ContactNo == staff.ContactNo))
                    return new ResponseResult("Fail", "Contact number already exists");

                // ⭐ AUTO GENERATE PASSWORD
                string plainPassword = GeneratePassword();

                // ⭐ HASH PASSWORD
                var hasher = new PasswordHasher<StaffMaster>();
                staff.Password = hasher.HashPassword(staff, plainPassword);

                staff.CreatedAt = DateTime.Now;
                staff.Status = "Active";

                _dbContext.StaffMasters.Add(staff);
                await _dbContext.SaveChangesAsync();

                // ⭐ EMAIL SEND
                try
                {
                    var emailService = new EmailService();
                    string emailBody = $@"
                    <html>
                    <body style='font-family: Arial;'>
                        <h2>Welcome to Abson Energy</h2>
                        <p>Dear {staff.FullName},</p>
                        <p>Your Staff account has been created successfully.</p>
                        <p><b>Email:</b> {staff.Email}</p>
                        <p><b>Password:</b> {plainPassword}</p>
                        <p>Please login and change your password immediately.</p>
                        <br/>
                        <p>Regards,<br/>Admin Team</p>
                    </body>
                    </html>";

                    await emailService.SendEmailAsync(staff.Email, "Staff Account Created", emailBody);
                }
                catch (Exception ex)
                {
                    // If email fails, user is still saved, but we notify about the email failure
                    return new ResponseResult("OK", "Staff saved successfully, but password email failed: " + ex.Message);
                }

                return new ResponseResult("OK", "Staff saved successfully. Password sent to email.");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ListStaff()
        {
            try
            {
                var data = await _dbContext.StaffMasters.ToListAsync();
                return new ResponseResult("OK", data);
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> DetailStaff(int id)
        {
            try
            {
                var data = await _dbContext.StaffMasters.FindAsync(id);
                if (data == null) return new ResponseResult("Fail", "Staff not found");
                return new ResponseResult("OK", data);
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> UpdateStaff(StaffMaster staff)
        {
            try
            {
                var existing = await _dbContext.StaffMasters.FindAsync(staff.Id);
                if (existing == null) return new ResponseResult("Fail", "Staff not found");

                if (string.IsNullOrWhiteSpace(staff.FullName)) return new ResponseResult("Fail", "Full Name is required");
                if (string.IsNullOrWhiteSpace(staff.Email)) return new ResponseResult("Fail", "Email is required");

                existing.FullName = staff.FullName;
                existing.Address = staff.Address;
                existing.Email = staff.Email;
                existing.ContactNo = staff.ContactNo;
                existing.Status = staff.Status;
                existing.Role = staff.Role;

                await _dbContext.SaveChangesAsync();
                return new ResponseResult("OK", "Staff updated successfully");
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> DeleteStaff(int id)
        {
            try
            {
                var existing = await _dbContext.StaffMasters.FindAsync(id);
                if (existing == null) return new ResponseResult("Fail", "Staff not found");
                _dbContext.StaffMasters.Remove(existing);
                await _dbContext.SaveChangesAsync();
                return new ResponseResult("OK", "Staff deleted successfully");
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> Login(string email, string password)
        {
            try
            {
                var staff = await _dbContext.StaffMasters.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
                if (staff == null) return new ResponseResult("Fail", "Invalid Email or Password");
                if (staff.Status != "Active") return new ResponseResult("Fail", "Account disabled.");

                var hasher = new PasswordHasher<StaffMaster>();
                var result = hasher.VerifyHashedPassword(staff, staff.Password, password);

                if (result == PasswordVerificationResult.Failed) return new ResponseResult("Fail", "Invalid Email or Password");

                staff.Password = null;
                return new ResponseResult("OK", staff);
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> ChangePassword(int staffId, string oldPassword, string newPassword)
        {
            try
            {
                var staff = await _dbContext.StaffMasters.FindAsync(staffId);
                if (staff == null) return new ResponseResult("Fail", "Staff not found");

                var hasher = new PasswordHasher<StaffMaster>();
                var verify = hasher.VerifyHashedPassword(staff, staff.Password, oldPassword);
                if (verify == PasswordVerificationResult.Failed) return new ResponseResult("Fail", "Old password wrong");

                staff.Password = hasher.HashPassword(staff, newPassword);
                await _dbContext.SaveChangesAsync();
                return new ResponseResult("OK", "Password changed.");
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }

        public async Task<ResponseResult> ForgotPassword(string email)
        {
            try
            {
                var staff = await _dbContext.StaffMasters.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
                if (staff == null) return new ResponseResult("Fail", "Email not found");

                string newPass = GeneratePassword();
                var hasher = new PasswordHasher<StaffMaster>();
                staff.Password = hasher.HashPassword(staff, newPass);
                await _dbContext.SaveChangesAsync();

                var emailService = new EmailService();
                await emailService.SendEmailAsync(staff.Email, "Staff Password Reset", $"Your new password: {newPass}");

                return new ResponseResult("OK", "New password sent to email.");
            }
            catch (Exception ex) { return new ResponseResult("Fail", ex.Message); }
        }
    }
}