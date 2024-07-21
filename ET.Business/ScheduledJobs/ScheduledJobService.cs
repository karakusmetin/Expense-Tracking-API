using ET.Base.Response;
using ET.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ET.Business.ScheduledJobs
{
    public class ScheduledJobService : IScheduledJobService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScheduledJobService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<ApiResponse<object>> AggregateDailyExpenses()
        {
            return await AggregateExpenses(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
        }

        public async Task<ApiResponse<object>> AggregateWeeklyExpenses()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            return await AggregateExpenses(startOfWeek, startOfWeek.AddDays(7));
        }

        public async Task<ApiResponse<object>> AggregateMonthlyExpenses()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await AggregateExpenses(startOfMonth, startOfMonth.AddMonths(1));
        }

        private async Task<ApiResponse<object>> AggregateExpenses(DateTime startDate, DateTime endDate)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ETDbContext>();
                var aggregates = await dbContext.Transactions
                    .Where(e => e.TransactionDate >= startDate && e.TransactionDate < endDate)
                    .GroupBy(e => e.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        TotalAmount = g.Sum(e => e.Amount),
                        Period = startDate.ToString("yyyy-MM-dd") + " to " + endDate.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                if (aggregates.Any())
                {
                    return new ApiResponse<object>(aggregates)
                    {
                        Success = true,
                        Message = "Expenses aggregated successfully."
                    };
                }
                else
                {
                    return new ApiResponse<object>("No transactions found for the given period.");
                }
            }
        }
    }
}
