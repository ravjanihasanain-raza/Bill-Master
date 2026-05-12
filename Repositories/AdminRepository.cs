using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Bill_Master.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AdminRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ PASSWORD GENERATOR METHOD (NEW)
        private string GeneratePassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ResponseResult> SaveAdmin(Admin admin)
        {
            try
            {
                // 🔹 Email Duplicate Check
                var emailExists = await _dbContext.Admins
                    .AnyAsync(x => x.Email.ToLower() == admin.Email.ToLower());

                if (emailExists)
                {
                    return new ResponseResult("Fail", "Email already exists");
                }

                // 🔹 Contact Number Duplicate Check
                var contactExists = await _dbContext.Admins
                    .AnyAsync(x => x.ContactNo == admin.ContactNo);

                if (contactExists)
                {
                    return new ResponseResult("Fail", "Contact number already exists");
                }

                // ⭐ AUTO GENERATE PASSWORD
                string plainPassword = GeneratePassword();

                // ⭐ HASH PASSWORD
                var hasher = new PasswordHasher<Admin>();
                admin.Password = hasher.HashPassword(admin, plainPassword);

                _dbContext.Admins.Add(admin);
                await _dbContext.SaveChangesAsync();

                // ⭐ EMAIL SEND
                var emailService = new EmailService();

                string emailBody = $@"
                <html>
                <body style='font-family: Arial;'>
                    <h2>Welcome to Admin Panel</h2>

                    <p>Dear {admin.FullName},</p>

                    <p>Your admin account has been created successfully.</p>

                    <p><b>Email:</b> {admin.Email}</p>
                    <p><b>Password:</b> {plainPassword}</p>

                    <p>Please login and change your password immediately.</p>

                    <br/>

                    <p>Regards,<br/>Admin Team</p>
                </body>
                </html>";

                await emailService.SendEmailAsync(
                    admin.Email,
                    "Admin Account Created",
                    emailBody
                );

                return new ResponseResult("OK",
                    "Admin saved successfully. Password sent to email.");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ListAdmin()
        {
            try
            {
                var data = await _dbContext.Admins.ToListAsync();
                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DetailAdmin(int id)
        {
            try
            {
                var data = await _dbContext.Admins.FindAsync(id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Admin Not Found");
                }

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> DeleteAdmin(int id)
        {
            try
            {
                var data = await _dbContext.Admins.FindAsync(id);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Admin Not Found");
                }

                _dbContext.Admins.Remove(data);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Admin Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> UpdateAdmin(Admin admin)
        {
            try
            {
                var existingAdmin = await _dbContext.Admins.FindAsync(admin.Id);

                if (existingAdmin == null)
                {
                    return new ResponseResult("Fail", "Admin Not Found");
                }

                // 🔹 Email Duplicate Check (excluding current record)
                var emailExists = await _dbContext.Admins
                    .AnyAsync(x => x.Email.ToLower() == admin.Email.ToLower()
                                && x.Id != admin.Id);

                if (emailExists)
                {
                    return new ResponseResult("Fail", "Email already exists");
                }

                // 🔹 Contact Duplicate Check (excluding current record)
                var contactExists = await _dbContext.Admins
                    .AnyAsync(x => x.ContactNo == admin.ContactNo
                                && x.Id != admin.Id);

                if (contactExists)
                {
                    return new ResponseResult("Fail", "Contact number already exists");
                }

                // 🔹 Update fields
                existingAdmin.FullName = admin.FullName;
                existingAdmin.ContactNo = admin.ContactNo;
                existingAdmin.Email = admin.Email;

                // 🔐 PASSWORD UPDATE LOGIC
                if (admin.Password != null)
                {
                    if (string.IsNullOrWhiteSpace(admin.Password))
                    {
                        return new ResponseResult("Fail", "Password cannot be empty");
                    }

                    var hasher = new PasswordHasher<Admin>();
                    existingAdmin.Password = hasher.HashPassword(existingAdmin, admin.Password);
                }

                existingAdmin.Status = admin.Status;
                existingAdmin.Role = admin.Role;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Admin Updated Successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    
        public async Task<ResponseResult> Login(string email, string password)
        {
            try
            {
                var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

                if (admin == null)
                {
                    return new ResponseResult("Fail", "Invalid Email or Password");
                }

                var hasher = new PasswordHasher<Admin>();
                var result = hasher.VerifyHashedPassword(admin, admin.Password, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return new ResponseResult("Fail", "Invalid Email or Password");
                }

                // Don't return the hashed password in the response for security
                admin.Password = null;
                return new ResponseResult("OK", admin);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ChangePassword(int adminId, string oldPassword, string newPassword)
        {
            try
            {
                var admin = await _dbContext.Admins.FindAsync(adminId);

                if (admin == null)
                {
                    return new ResponseResult("Fail", "Admin Not Found");
                }

                var hasher = new PasswordHasher<Admin>();

                // Verify the old password first
                var verificationResult = hasher.VerifyHashedPassword(admin, admin.Password, oldPassword);
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return new ResponseResult("Fail", "Incorrect Old Password");
                }

                // Hash and set the new password
                admin.Password = hasher.HashPassword(admin, newPassword);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Password Changed Successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> ForgotPassword(string email)
        {
            try
            {
                var admin = await _dbContext.Admins.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

                if (admin == null)
                {
                    return new ResponseResult("Fail", "Email address is not registered");
                }

                // Generate new random password
                string newPlainPassword = GeneratePassword();

                // Hash the new password and save it
                var hasher = new PasswordHasher<Admin>();
                admin.Password = hasher.HashPassword(admin, newPlainPassword);
                await _dbContext.SaveChangesAsync();

                // Send the new password via Email
                var emailService = new EmailService();
                string emailBody = $@"
                <html>
                <body style='font-family: Arial;'>
                    <h2>Password Reset Request</h2>

                    <p>Dear {admin.FullName},</p>

                    <p>Your password has been reset successfully.</p>

                    <p><b>Your New Password:</b> {newPlainPassword}</p>

                    <p>Please login and change your password immediately for security reasons.</p>

                    <br/>

                    <p>Regards,<br/>Admin Team</p>
                </body>
                </html>";

                await emailService.SendEmailAsync(
                    admin.Email,
                    "Admin Password Reset",
                    emailBody
                );

                return new ResponseResult("OK", "A new password has been sent to your email.");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}