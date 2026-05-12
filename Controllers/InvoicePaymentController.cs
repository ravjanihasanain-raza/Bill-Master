using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicePaymentController : ControllerBase
    {
        private readonly IInvoicePayment _paymentRepo;

        public InvoicePaymentController(IInvoicePayment paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        // ⭐ SAVE PAYMENT
        [HttpPost("Save")]
        public async Task<IActionResult> SavePayment([FromBody] InvoicePayment payment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _paymentRepo.SavePayment(payment);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
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

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ DETAIL BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailPayment(int id)
        {
            try
            {
                var result = await _paymentRepo.DetailPayment(id);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }


        [HttpGet("ListByInvoice/{invoiceId}")]
        public async Task<IActionResult> ListByInvoice(int invoiceId)
        {
            try
            {
                var result = await _paymentRepo.ListByInvoice(invoiceId);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ UPDATE PAYMENT
        [HttpPut("Update")]
        public async Task<IActionResult> UpdatePayment([FromBody] InvoicePayment payment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _paymentRepo.UpdatePayment(payment);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
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