using Bill_Master.Interfaces;
using Bill_Master.Model;
using Bill_Master.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _repo;

        public StockController(IStockRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("LowStock")]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _repo.GetLowStockProducts();
            return Ok(result);
        }
        [HttpGet("History/{productId}")]
        public async Task<IActionResult> GetStockHistory(int productId)
        {
            try
            {
                var result = await _repo.GetStockHistory(productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    }
}