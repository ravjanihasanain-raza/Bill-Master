using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceItemsController : ControllerBase
    {
        private readonly IInvoiceItems _invoiceItems;

        public InvoiceItemsController(IInvoiceItems invoiceItems)
        {
            _invoiceItems = invoiceItems;
        }

        // ✅ SAVE
        [HttpPost("Save")]
        public async Task<IActionResult> SaveInvoiceItemSave([FromBody] InvoiceItems item)
        {
            try
            {
                var result = await _invoiceItems.SaveInvoiceItem(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ LIST
        [HttpGet("List")]
        public async Task<IActionResult> ListInvoiceItems()
        {
            try
            {
                var result = await _invoiceItems.ListInvoiceItems();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ DETAIL
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailInvoiceItem(int id)
        {
            try
            {
                var result = await _invoiceItems.DetailInvoiceItem(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ UPDATE
        [HttpPut("Update")]
        public async Task<IActionResult> UpdUpdateInvoiceItemate([FromBody] InvoiceItems item)
        {
            try
            {
                var result = await _invoiceItems.UpdateInvoiceItem(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ DELETE
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteInvoiceItem(int id)
        {
            try
            {
                var result = await _invoiceItems.DeleteInvoiceItem(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}