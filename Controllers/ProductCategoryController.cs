using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategory _repo;

        public ProductCategoryController(IProductCategory repo)
        {
            _repo = repo;
        }

        // ⭐ SAVE CATEGORY
        [HttpPost("Save")]
        public async Task<IActionResult> SaveCategory([FromBody] ProductCategory category)
        {
            try
            {
                var result = await _repo.SaveCategory(category);

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

        // ⭐ LIST CATEGORY
        [HttpGet("List")]
        public async Task<IActionResult> ListCategory()
        {
            try
            {
                var result = await _repo.ListCategory();

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
        public async Task<IActionResult> DetailCategory(int id)
        {
            try
            {
                var result = await _repo.DetailCategory(id);

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

        // ⭐ UPDATE CATEGORY
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory([FromBody] ProductCategory category)
        {
            try
            {
                var result = await _repo.UpdateCategory(category);

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

        // ⭐ DELETE CATEGORY (Soft Delete)
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _repo.DeleteCategory(id);

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
