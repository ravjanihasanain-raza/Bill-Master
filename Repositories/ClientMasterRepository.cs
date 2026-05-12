using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bill_Master.Repositories
{
    public class ClientMasterRepository : IClientMaster
    {
        private readonly ApplicationDBContext _dbContext;

        public ClientMasterRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE CLIENT
        public async Task<ResponseResult> SaveClient(ClientMaster client)
        {
            try
            {
                // 🔴 Email format
                //if (!Regex.IsMatch(client.Email,
                //    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                //    return new ResponseResult("Fail", "Invalid email format");

                // 🔴 Contact number (10 digits)
                //if (!Regex.IsMatch(client.ContactNo, @"^\d{10}$"))
                //    return new ResponseResult("Fail",
                //        "Contact number must be 10 digits");

                // 🔴 GstIN format
                if (!Regex.IsMatch(client.GstIN,
                    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$"))
                    return new ResponseResult("Fail", "Invalid GstIN");

                // 🔴 Duplicate Email
                if (await _dbContext.ClientMasters
                    .AnyAsync(x => x.Email.ToLower() == client.Email.ToLower()))
                    return new ResponseResult("Fail",
                        "Email already exists");

                // 🔴 Duplicate Contact
                if (await _dbContext.ClientMasters
                    .AnyAsync(x => x.ContactNo == client.ContactNo))
                    return new ResponseResult("Fail",
                        "Contact number already exists");

                var staffExists = await _dbContext.StaffMasters
    .AnyAsync(x => x.Id == client.StaffMasterId);

                if (!staffExists)
                    return new ResponseResult("Fail", "Invalid staff");

                client.CreatedAt = DateTime.Now;

                _dbContext.ClientMasters.Add(client);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Client saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST CLIENTS
        public async Task<ResponseResult> ListClient()
        {
            try
            {
                var data = await _dbContext.ClientMasters.ToListAsync();
                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DETAIL BY ID
        public async Task<ResponseResult> DetailClient(int id)
        {
            try
            {
                var data = await _dbContext.ClientMasters.FindAsync(id);

                if (data == null)
                    return new ResponseResult("Fail", "Client not found");

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE CLIENT
        public async Task<ResponseResult> UpdateClient(ClientMaster client)
        {
            try
            {
                var existing = await _dbContext.ClientMasters.FindAsync(client.Id);

                if (existing == null)
                    return new ResponseResult("Fail", "Client not found");

                // 🔴 Email format
                if (!Regex.IsMatch(client.Email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return new ResponseResult("Fail", "Invalid email format");

                // 🔴 Contact number
                if (!Regex.IsMatch(client.ContactNo, @"^\d{10}$"))
                    return new ResponseResult("Fail",
                        "Contact number must be 10 digits");

                // 🔴 GstIN format
                if (!Regex.IsMatch(client.GstIN,
                    @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][1-9A-Z]Z[0-9A-Z]$"))
                    return new ResponseResult("Fail", "Invalid GstIN");

                // 🔴 Duplicate Email (exclude current)
                if (await _dbContext.ClientMasters
                    .AnyAsync(x => x.Email.ToLower() == client.Email.ToLower()
                                && x.Id != client.Id))
                    return new ResponseResult("Fail",
                        "Email already exists");

                // 🔴 Duplicate Contact
                if (await _dbContext.ClientMasters
                    .AnyAsync(x => x.ContactNo == client.ContactNo
                                && x.Id != client.Id))
                    return new ResponseResult("Fail",
                        "Contact number already exists");

                // 🔄 Update fields
                existing.BusinessName = client.BusinessName;
                existing.Address = client.Address;
                existing.Email = client.Email;
                existing.ContactNo = client.ContactNo;
                existing.GstIN = client.GstIN;
                existing.StateCode = client.StateCode;
                existing.State = client.State;
                existing.ContactPerson = client.ContactPerson;
                existing.StaffMasterId = client.StaffMasterId;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Client updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ DELETE CLIENT
        public async Task<ResponseResult> DeleteClient(int id)
        {
            try
            {
                var existing = await _dbContext.ClientMasters.FindAsync(id);

                if (existing == null)
                    return new ResponseResult("Fail", "Client not found");

                _dbContext.ClientMasters.Remove(existing);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Client deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}
