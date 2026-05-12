using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseItemsController : ControllerBase
    {
        private readonly IPurchaseItems _itemRepo;

        public PurchaseItemsController(IPurchaseItems itemRepo)
        {
            _itemRepo = itemRepo;
        }

        // ⭐ SAVE ITEM
        [HttpPost("Save")]
        public async Task<IActionResult> SaveItem([FromBody] PurchaseItems item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _itemRepo.SaveItem(item);

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

        // ⭐ LIST ITEMS
        [HttpGet("List")]
        public async Task<IActionResult> ListItem()
        {
            try
            {
                var result = await _itemRepo.ListItem();

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

        // ⭐ DETAIL BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailItem(int id)
        {
            try
            {
                var result = await _itemRepo.DetailItem(id);

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

        // ⭐ UPDATE ITEM
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateItem([FromBody] PurchaseItems item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _itemRepo.UpdateItem(item);

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

        // ⭐ DELETE ITEM
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                var result = await _itemRepo.DeleteItem(id);

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
