using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialYearController : ControllerBase
    {
        private readonly IFinancialYear _financialYearRepo;

        public FinancialYearController(IFinancialYear financialYearRepo)
        {
            _financialYearRepo = financialYearRepo;
        }

        // ⭐ SAVE FINANCIAL YEAR
        [HttpPost("Save")]
        public async Task<IActionResult> SaveFinancialYear([FromBody] FinancialYear year)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _financialYearRepo.SaveFinancialYear(year);

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

        // ⭐ LIST ALL YEARS
        [HttpGet("List")]
        public async Task<IActionResult> ListFinancialYear()
        {
            try
            {
                var result = await _financialYearRepo.ListFinancialYear();

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

        // ⭐ GET BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailFinancialYear(int id)
        {
            try
            {
                var result = await _financialYearRepo.DetailFinancialYear(id);

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

        // ⭐ UPDATE
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateFinancialYear([FromBody] FinancialYear year)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _financialYearRepo.UpdateFinancialYear(year);

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

        // ⭐ SOFT DELETE
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteFinancialYear(int id)
        {
            try
            {
                var result = await _financialYearRepo.DeleteFinancialYear(id);

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

        // SET ACTIVE
        [HttpPut("SetActive/{id}")]
        public async Task<IActionResult> SetActive(int id)
        {
            try
            {
                var result = await _financialYearRepo.SetActiveYear(id);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }

        // CLOSE YEAR
        [HttpPut("CloseYear/{id}")]
        public async Task<IActionResult> CloseYear(int id)
        {
            try
            {
                var result = await _financialYearRepo.CloseYear(id);

                if (result.Status == "OK")
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult("Fail", ex.Message));
            }
        }
    }
}
