using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseMasterController : ControllerBase
    {
        private readonly IExpenseMaster _repo;

        public ExpenseMasterController(IExpenseMaster repo)
        {
            _repo = repo;
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] ExpenseMaster model)
        {
            try
            {
                var result = await _repo.SaveExpense(model);
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
                var result = await _repo.ListExpense();
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
                var result = await _repo.DetailExpense(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ExpenseMaster model)
        {
            try
            {
                var result = await _repo.UpdateExpense(model);
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
                var result = await _repo.DeleteExpense(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("MarkPaid/{id}")]
        public async Task<IActionResult> MarkPaid(int id)
        {
            try
            {
                var result = await _repo.MarkPaid(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}