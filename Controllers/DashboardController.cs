using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bill_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _repo;

        public DashboardController(IDashboardRepository repo)
        {
            _repo = repo;
        }

        // ════════════════════════════════════════════════════
        //  GET  api/dashboard
        //
        //  Query-string parameters (all optional):
        //  ┌─────────────┬──────────────────────────────────────────────┐
        //  │ filterType  │ "today" | "month" | "year" | "custom"        │
        //  │             │  default → "year"                            │
        //  ├─────────────┼──────────────────────────────────────────────┤
        //  │ year        │ e.g. 2025   (default → current year)         │
        //  ├─────────────┼──────────────────────────────────────────────┤
        //  │ month       │ 1–12        (used when filterType="month")   │
        //  │             │  default → current month                     │
        //  ├─────────────┼──────────────────────────────────────────────┤
        //  │ fromDate    │ yyyy-MM-dd  (used when filterType="custom")  │
        //  │ toDate      │ yyyy-MM-dd                                   │
        //  └─────────────┴──────────────────────────────────────────────┘
        //
        //  Examples:
        //    /api/dashboard                          → current year
        //    /api/dashboard?filterType=today         → today only
        //    /api/dashboard?filterType=month&year=2025&month=4
        //    /api/dashboard?filterType=year&year=2024
        //    /api/dashboard?filterType=custom&fromDate=2025-01-01&toDate=2025-03-31
        // ════════════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string filterType = "year",
            [FromQuery] int? year = null,
            [FromQuery] int? month = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var filter = new DashboardFilter
                {
                    FilterType = filterType?.ToLower()?.Trim() ?? "year",
                    Year = year,
                    Month = month,
                    FromDate = fromDate,
                    ToDate = toDate
                };

                var result = await _repo.GetDashboard(filter);

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
