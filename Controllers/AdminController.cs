using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _manageAdmin;

        public AdminController(IAdminRepository manageAdmin)
        {
            _manageAdmin = manageAdmin;
        }

        // ⭐ SEND EMAIL API
        //[HttpPost("SendEmail")]
        //public async Task<IActionResult> SendEmail([FromBody] string email)
        //{
        //    try
        //    {
        //        var admin = await _manageAdmin.GetAdminByEmail(email);

        //        if (admin == null)
        //        {
        //            return BadRequest(new ResponseResult("Fail", "Email not found"));
        //        }

        //        // password generate
        //        var password = new Random().Next(100000, 999999).ToString();

        //        var emailService = new EmailService();

        //        string body = $@"
        //<h2>Admin Login</h2>
        //<p>Email: <b>{email}</b></p>
        //<p>Password: <b>{password}</b></p>";

        //        await emailService.SendEmailAsync(
        //            email,
        //            "Admin Login Password",
        //            body
        //        );

        //        return Ok(new ResponseResult("OK", "Email Sent"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseResult("Fail", ex.Message));
        //    }
        //}

        // ⭐ SAVE ADMIN (Auto Password Generate + Hash)
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAdmin([FromBody] Admin admin)
        {
            try
            {
                // Model validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _manageAdmin.SaveAdmin(admin);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ LIST ALL ADMINS
        [HttpGet("List")]
        public async Task<IActionResult> ListAdmin()
        {
            try
            {
                var result = await _manageAdmin.ListAdmin();

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ GET ADMIN BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailAdmin(int id)
        {
            try
            {
                var result = await _manageAdmin.DetailAdmin(id);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ DELETE ADMIN
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var result = await _manageAdmin.DeleteAdmin(id);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ UPDATE ADMIN
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAdmin([FromBody] Admin admin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _manageAdmin.UpdateAdmin(admin);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    

        // ⭐ LOGIN
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var result = await _manageAdmin.Login(request.Email, request.Password);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ CHANGE PASSWORD
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            try
            {
                var result = await _manageAdmin.ChangePassword(request.AdminId, request.OldPassword, request.NewPassword);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ FORGOT PASSWORD
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            try
            {
                var result = await _manageAdmin.ForgotPassword(request.Email);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    }
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordDto
    {
        public int AdminId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
}


    

