using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController : ControllerBase
    {
        private readonly IExpenseCategory _repo;

        public ExpenseCategoryController(IExpenseCategory repo)
        {
            _repo = repo;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] ExpenseCategory model)
        {
            try
            {
                var result = await _repo.SaveExpenseCategory(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _repo.ListExpenseCategory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var result = await _repo.DetailExpenseCategory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ExpenseCategory model)
        {
            try
            {
                var result = await _repo.UpdateExpenseCategory(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _repo.DeleteExpenseCategory(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}