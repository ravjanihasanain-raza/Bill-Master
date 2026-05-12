using Bill_Master.Interfaces;
using Bill_Master.Model;
using Bill_Master.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffMasterController : ControllerBase
    {
        private readonly IStaffMaster _staffRepo;

        public StaffMasterController(IStaffMaster staffRepo)
        {
            _staffRepo = staffRepo;
        }


        // ⭐ SAVE STAFF
        [HttpPost("Save")]
        public async Task<IActionResult> SaveStaff([FromBody] StaffMaster staff)
        {
            try
            {
                var result = await _staffRepo.SaveStaff(staff);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ LIST ALL STAFF
        [HttpGet("List")]
        public async Task<IActionResult> ListStaff()
        {
            try
            {
                var result = await _staffRepo.ListStaff();

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ GET STAFF BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailStaff(int id)
        {
            try
            {
                var result = await _staffRepo.DetailStaff(id);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ UPDATE STAFF
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateStaff([FromBody] StaffMaster staff)
        {
            try
            {
                var result = await _staffRepo.UpdateStaff(staff);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ DELETE STAFF
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            try
            {
                var result = await _staffRepo.DeleteStaff(id);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var result = await _staffRepo.Login(request.Email, request.Password);
                if (result.Status == "OK") return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex) { return StatusCode(500, new ResponseResult("Fail", ex.Message)); }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] StaffChangePasswordDto request)
        {
            try
            {
                var result = await _staffRepo.ChangePassword(request.StaffId, request.OldPassword, request.NewPassword);
                if (result.Status == "OK") return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex) { return StatusCode(500, new ResponseResult("Fail", ex.Message)); }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            try
            {
                var result = await _staffRepo.ForgotPassword(request.Email);
                if (result.Status == "OK") return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex) { return StatusCode(500, new ResponseResult("Fail", ex.Message)); }
        }
    }

    // ==========================================
    // ⭐ DTOs FOR REQUEST BINDING
    // ==========================================

    // (If LoginDto and ForgotPasswordDto already exist in your project from Admin, 
    // you don't need to redeclare them here. Just defining them in case they are missing)
    /*
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; }
    }
    */

    public class StaffChangePasswordDto
    {
        public int StaffId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

