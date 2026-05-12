using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InwardStockController : ControllerBase
    {
        private readonly IInwardStock _inwardRepo;

        public InwardStockController(IInwardStock inwardRepo)
        {
            _inwardRepo = inwardRepo;
        }

        // ⭐ SAVE INWARD
        [HttpPost("Save")]
        public async Task<IActionResult> SaveInward([FromBody] InwardStock inward)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _inwardRepo.SaveInward(inward);

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

        // ⭐ LIST ALL INWARD RECORDS
        [HttpGet("List")]
        public async Task<IActionResult> ListInward()
        {
            try
            {
                var result = await _inwardRepo.ListInward();

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

        // ⭐ GET INWARD BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailInward(int id)
        {
            try
            {
                var result = await _inwardRepo.DetailInward(id);

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

        // ⭐ UPDATE INWARD
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateInward([FromBody] InwardStock inward)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _inwardRepo.UpdateInward(inward);

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

        // ⭐ DELETE INWARD
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteInward(int id)
        {
            try
            {
                var result = await _inwardRepo.DeleteInward(id);

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
    }
}