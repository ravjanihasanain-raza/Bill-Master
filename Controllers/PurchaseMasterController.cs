using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseMasterController : ControllerBase
    {
        private readonly IPurchaseMaster _purchaseRepo;

        public PurchaseMasterController(IPurchaseMaster purchaseRepo)
        {
            _purchaseRepo = purchaseRepo;
        }

        // ⭐ SAVE PURCHASE

        [HttpPost("Save")]
        public async Task<IActionResult> SavePurchase([FromBody] PurchaseMaster purchase)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _purchaseRepo.SavePurchase(purchase);

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ LIST PURCHASES
        [HttpGet("List")]
        public async Task<IActionResult> ListPurchase()
        {
            try
            {
                var result = await _purchaseRepo.ListPurchase();

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

        // ⭐ GET PURCHASE BY ID (DETAIL)
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailPurchase(int id)
        {
            try
            {
                var result = await _purchaseRepo.DetailPurchase(id);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ UPDATE PURCHASE
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePurchase([FromBody] PurchaseMaster purchase)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _purchaseRepo.UpdatePurchase(purchase);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ DELETE PURCHASE
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePurchase([FromRoute] int id) // 🔥 FIX: Added [FromRoute]
        {
            try
            {
                var result = await _purchaseRepo.DeletePurchase(id);

                if (result.Status == "OK")
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    }
}
