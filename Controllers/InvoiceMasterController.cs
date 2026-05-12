using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceMasterController : ControllerBase
    {
        private readonly IInvoiceMaster _invoiceRepository;

        public InvoiceMasterController(IInvoiceMaster invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        // ✅ SAVE
        [HttpPost("SaveFullInvoice")]
        public async Task<IActionResult> SaveFullInvoice([FromBody] InvoiceWithItemsDto data)
        {
            try
            {
                var result = await _invoiceRepository.SaveFullInvoice(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ LIST
        [HttpGet("ListInvoice")]
        public async Task<IActionResult> ListInvoice()
        {
            try
            {
                var result = await _invoiceRepository.ListInvoice();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DETAIL BY ID
        [HttpGet("DetailInvoice/{id}")]
        public async Task<IActionResult> DetailInvoice(int id)
        {
            try
            {
                var result = await _invoiceRepository.DetailInvoice(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ UPDATE
        [HttpPut("UpdateInvoice")]
        public async Task<IActionResult> UpdateInvoice([FromBody] InvoiceWithItemsDto data)
        {
            try
            {
                var result = await _invoiceRepository.UpdateInvoice(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseResult("Fail", ex.Message));
            }
        }

        // ✅ DELETE
        [HttpDelete("DeleteInvoice/{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                var result = await _invoiceRepository.DeleteInvoice(id);
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