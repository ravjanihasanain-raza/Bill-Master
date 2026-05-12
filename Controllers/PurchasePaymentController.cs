using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasePaymentController : ControllerBase
    {
        private readonly IPurchasePayment _paymentRepo;

        public PurchasePaymentController(IPurchasePayment paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        // ⭐ SAVE PAYMENT
        [HttpPost("Save")]
        public async Task<IActionResult> SavePayment([FromBody] PurchasePayment payment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _paymentRepo.SavePayment(payment);

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

        // ⭐ LIST PAYMENTS
        [HttpGet("List")]
        public async Task<IActionResult> ListPayment()
        {
            try
            {
                var result = await _paymentRepo.ListPayment();

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

        // ⭐ GET PAYMENT BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailPayment(int id)
        {
            try
            {
                var result = await _paymentRepo.DetailPayment(id);

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

        // ⭐ UPDATE PAYMENT
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePayment([FromBody] PurchasePayment payment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _paymentRepo.UpdatePayment(payment);

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

        // ⭐ DELETE PAYMENT
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentRepo.DeletePayment(id);

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