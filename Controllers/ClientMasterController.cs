using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientMasterController : ControllerBase
    {
        private readonly IClientMaster _clientRepo;

        public ClientMasterController(IClientMaster clientRepo)
        {
            _clientRepo = clientRepo;
        }

        // ⭐ SAVE CLIENT
        [HttpPost("Save")]
        public async Task<IActionResult> SaveClient([FromBody] ClientMaster client)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _clientRepo.SaveClient(client);

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

        // ⭐ LIST ALL CLIENTS
        [HttpGet("List")]
        public async Task<IActionResult> ListClient()
        {
            try
            {
                var result = await _clientRepo.ListClient();

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

        // ⭐ GET CLIENT BY ID
        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> DetailClient(int id)
        {
            try
            {
                var result = await _clientRepo.DetailClient(id);

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

        // ⭐ UPDATE CLIENT
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateClient([FromBody] ClientMaster client)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _clientRepo.UpdateClient(client);

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

        // ⭐ DELETE CLIENT
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await _clientRepo.DeleteClient(id);

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
