using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockUsedController : ControllerBase
    {
        private readonly IStockUsed _stockUsedRepository;

        public StockUsedController(IStockUsed stockUsedRepository)
        {
            _stockUsedRepository = stockUsedRepository;
        }

        // ✅ SAVE
        [HttpPost("SaveStockUsed")]
        public async Task<IActionResult> SaveStockUsed([FromBody] StockUsed stockUsed)
        {
            try
            {
                var result = await _stockUsedRepository.SaveStockUsed(stockUsed);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ LIST
        [HttpGet("ListStockUsed")]
        public async Task<IActionResult> ListStockUsed()
        {
            try
            {
                var result = await _stockUsedRepository.ListStockUsed();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DETAIL BY ID
        [HttpGet("DetailStockUsed/{id}")]
        public async Task<IActionResult> DetailStockUsed(int id)
        {
            try
            {
                var result = await _stockUsedRepository.DetailStockUsed(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ UPDATE
        [HttpPut("UpdateStockUsed")]
        public async Task<IActionResult> UpdateStockUsed([FromBody] StockUsed stockUsed)
        {
            try
            {
                var result = await _stockUsedRepository.UpdateStockUsed(stockUsed);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DELETE
        [HttpDelete("DeleteStockUsed/{id}")]
        public async Task<IActionResult> DeleteStockUsed(int id)
        {
            try
            {
                var result = await _stockUsedRepository.DeleteStockUsed(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }
    }
}