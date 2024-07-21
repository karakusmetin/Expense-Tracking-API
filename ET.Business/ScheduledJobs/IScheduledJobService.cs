using ET.Base.Response;
using ET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Business.ScheduledJobs
{
    public interface IScheduledJobService
    {
        Task<ApiResponse<object>> AggregateDailyExpenses();
        Task<ApiResponse<object>> AggregateWeeklyExpenses();
        Task<ApiResponse<object>> AggregateMonthlyExpenses();
    }

}
