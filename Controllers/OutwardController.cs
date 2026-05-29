using Bill_Master.Interfaces;
using Bill_Master.Model; // 🔥 FIX: Removed wrong namespace
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutwardController : ControllerBase
    {
        private readonly IOutward _outwardRepository;

        public OutwardController(IOutward outwardRepository)
        {
            _outwardRepository = outwardRepository;
        }

        // ✅ SAVE OUTWARD
        [HttpPost("SaveOutward")]
        public async Task<IActionResult> SaveOutward([FromBody] Outward outward)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _outwardRepository.SaveOutward(outward);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ LIST OUTWARD
        [HttpGet("ListOutward")]
        public async Task<IActionResult> ListOutward()
        {
            try
            {
                var result = await _outwardRepository.ListOutward();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DETAIL BY ID
        [HttpGet("DetailOutward/{id}")]
        public async Task<IActionResult> DetailOutward(int id)
        {
            try
            {
                var result = await _outwardRepository.DetailOutward(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ UPDATE
        [HttpPut("UpdateOutward")]
        public async Task<IActionResult> UpdateOutward([FromBody] Outward outward)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _outwardRepository.UpdateOutward(outward);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DELETE
        [HttpDelete("DeleteOutward/{id}")]
        public async Task<IActionResult> DeleteOutward([FromRoute] int id)
        {
            try
            {
                var result = await _outwardRepository.DeleteOutward(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
        // ✅ AUTO OUTWARD FROM INVOICE
        //[HttpPost("AutoFromInvoice")]
        //public async Task<IActionResult> AutoFromInvoice([FromBody] InvoiceDto dto)
        //{
        //    try
        //    {
        //        if (dto == null || dto.Items == null || !dto.Items.Any())
        //        {
        //            return BadRequest(new ResponseResult("Fail", "Invalid invoice data"));
        //        }

        //        var result = await _outwardRepository.AutoFromInvoice(dto);

        //        if (result.Status != "OK")
        //        {
        //            return BadRequest(result);
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500,
        //            new ResponseResult("Fail", ex.InnerException?.Message ?? ex.Message));
        //    }
        //}
    }
}