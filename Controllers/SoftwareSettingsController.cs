using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoftwareSettingsController : ControllerBase
    {
        private readonly ISoftwareSettings _settingsRepo;

        public SoftwareSettingsController(ISoftwareSettings settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        // ⭐ SAVE OR UPDATE SETTINGS
        [HttpPost("Save")]
        public async Task<IActionResult> SaveSettings([FromBody] SoftwareSettings settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _settingsRepo.SaveSettings(settings);

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }

        // ⭐ GET SETTINGS (Single Record)
        [HttpGet("Get")]
        public async Task<IActionResult> GetSettings()
        {

            try
            {
                var result = await _settingsRepo.GetSettings();

                if (result.Status == "OK")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseResult("Fail", ex.Message));
            }
        }
    }
}
