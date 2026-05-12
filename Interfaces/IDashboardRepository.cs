using Bill_Master.Model;

namespace Bill_Master.Interfaces
{
    public interface IDashboardRepository
    {
        /// <summary>
        /// Returns all dashboard KPIs, charts and recent invoices
        /// filtered by the supplied DashboardFilter.
        /// </summary>
        Task<ResponseResult> GetDashboard(DashboardFilter filter);
    }
}
