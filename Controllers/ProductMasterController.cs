using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMasterController : ControllerBase
    {
        private readonly IProductMaster _productRepo;

        public ProductMasterController(IProductMaster productRepo)
        {
            _productRepo = productRepo;
        }

        // ⭐ SAVE PRODUCT
        [HttpPost("Save")]
        public async Task<IActionResult> SaveProduct([FromBody] ProductMaster product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _productRepo.SaveProduct(product);

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ LIST PRODUCTS
        [HttpGet("List")]
        public async Task<IActionResult> ListProduct()
        {
            try
            {
                var result = await _productRepo.ListProduct();

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ GET PRODUCT BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailProduct(int id)
        {
            try
            {
                var result = await _productRepo.DetailProduct(id);

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ UPDATE PRODUCT
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductMaster product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _productRepo.UpdateProduct(product);

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ DELETE PRODUCT
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productRepo.DeleteProduct(id);

                return result.Status == "OK"
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    }
}
