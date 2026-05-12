using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Microsoft.EntityFrameworkCore;

namespace Bill_Master.Repositories
{
    public class FinancialYearRepository : IFinancialYear
    {
        private readonly ApplicationDBContext _dbContext;

        public FinancialYearRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ⭐ SAVE FINANCIAL YEAR
        public async Task<ResponseResult> SaveFinancialYear(FinancialYear year)
        {
            try
            {
                // 🔴 FUTURE DATE CHECK
               

                // 🔴 EndDate must be after StartDate
                if (year.EndDate <= year.StartDate)
                {
                    return new ResponseResult("Fail",
                        "End date must be after start date");
                }
                // 🔴 YearName must match Start & End years
                string expectedYearName =
                    $"{year.StartDate.Year}-{year.EndDate.Year}";

                if (year.YearName != expectedYearName)
                {
                    return new ResponseResult("Fail",
                        $"Year name must be {expectedYearName}");
                }

                // 🔴 Duplicate Year Name Check
                var exists = await _dbContext.FinancialYears
                    .AnyAsync(x => x.YearName == year.YearName && !x.IsDelete);

                if (exists)
                {
                    return new ResponseResult("Fail",
                        "Financial year already exists");
                }

                var hasAnyYear = await _dbContext.FinancialYears.AnyAsync(x => !x.IsDelete);

                if (!hasAnyYear)
                {
                    year.IsActive = true;
                }

                year.CreatedAt = DateTime.Now;
                year.IsDelete = false;

                _dbContext.FinancialYears.Add(year);
                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK",
                    "Financial year saved successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ LIST ALL (NOT DELETED)
        public async Task<ResponseResult> ListFinancialYear()
        {
            try
            {
                var data = await _dbContext.FinancialYears
                    .Where(x => !x.IsDelete)
                    .ToListAsync();

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ GET BY ID
        public async Task<ResponseResult> DetailFinancialYear(int id)
        {
            try
            {
                var data = await _dbContext.FinancialYears
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete);

                if (data == null)
                {
                    return new ResponseResult("Fail", "Financial year not found");
                }

                return new ResponseResult("OK", data);
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ UPDATE
        public async Task<ResponseResult> UpdateFinancialYear(FinancialYear year)
        {
            try
            {
                var existing = await _dbContext.FinancialYears
                    .FirstOrDefaultAsync(x => x.Id == year.Id && !x.IsDelete);

                if (existing == null)
                {
                    return new ResponseResult("Fail", "Financial year not found");
                }

                // 🔴 YearName must match Start & End years
                string expectedYearName =
                    $"{year.StartDate.Year}-{year.EndDate.Year}";

                if (year.YearName != expectedYearName)
                {
                    return new ResponseResult("Fail",
                        $"Year name must be {expectedYearName}");
                }
                // FUTURE CHECK
                if (year.StartDate > DateTime.Today)
                {
                    return new ResponseResult("Fail",
                        "Future financial year not allowed");
                }

                // DATE LOGIC CHECK
                if (year.EndDate <= year.StartDate)
                {
                    return new ResponseResult("Fail",
                        "End date must be after start date");
                }


                // 🔴 Duplicate Year Name Check (excluding current)
                var exists = await _dbContext.FinancialYears
                    .AnyAsync(x => x.YearName == year.YearName
                                && x.Id != year.Id
                                && !x.IsDelete);

                if (exists)
                {
                    return new ResponseResult("Fail", "Financial year already exists");
                }

                existing.YearName = year.YearName;
                existing.StartDate = year.StartDate;
                existing.EndDate = year.EndDate;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Financial year updated successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        // ⭐ SOFT DELETE
        public async Task<ResponseResult> DeleteFinancialYear(int id)
        {
            try
            {
                var existing = await _dbContext.FinancialYears
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDelete);

                if (existing == null)
                {
                    return new ResponseResult("Fail", "Financial year not found");
                }

                existing.IsDelete = true;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Financial year deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> SetActiveYear(int id)
        {
            try
            {
                var allYears = await _dbContext.FinancialYears
                    .Where(x => !x.IsDelete)
                    .ToListAsync();

                foreach (var fy in allYears)
                {
                    fy.IsActive = false;
                }

                var selected = allYears.FirstOrDefault(x => x.Id == id);

                if (selected == null)
                    return new ResponseResult("Fail", "Financial Year Not Found");

                if (selected.IsClosed)
                    return new ResponseResult("Fail", "Closed year cannot activate");

                selected.IsActive = true;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Financial Year Activated");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }

        public async Task<ResponseResult> CloseYear(int id)
        {
            try
            {
                var fy = await _dbContext.FinancialYears
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (fy == null)
                    return new ResponseResult("Fail", "Not Found");

                fy.IsClosed = true;
                fy.IsActive = false;

                await _dbContext.SaveChangesAsync();

                return new ResponseResult("OK", "Year Closed");
            }
            catch (Exception ex)
            {
                return new ResponseResult("Fail", ex.Message);
            }
        }
    }
}
