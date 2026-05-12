using Bill_Master.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendor _repo;

        public VendorController(IVendor repo)
        {
            _repo = repo;
        }

        // ⭐ SAVE
        [HttpPost("Save")]
        public async Task<IActionResult> SaveVendor([FromBody] Vendor vendor)
        {
            var result = await _repo.SaveVendor(vendor);
            return result.Status == "OK" ? Ok(result) : BadRequest(result);
        }

        // ⭐ LIST
        [HttpGet("List")]
        public async Task<IActionResult> ListVendor()
        {
            var result = await _repo.ListVendor();
            return result.Status == "OK" ? Ok(result) : BadRequest(result);
        }

        // ⭐ DETAIL
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailVendor(int id)
        {
            var result = await _repo.DetailVendor(id);
            return result.Status == "OK" ? Ok(result) : BadRequest(result);
        }

        // ⭐ UPDATE
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateVendor([FromBody] Vendor vendor)
        {
            var result = await _repo.UpdateVendor(vendor);
            return result.Status == "OK" ? Ok(result) : BadRequest(result);
        }

        // ⭐ DELETE
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var result = await _repo.DeleteVendor(id);
            return result.Status == "OK" ? Ok(result) : BadRequest(result);
        }
    }
}
