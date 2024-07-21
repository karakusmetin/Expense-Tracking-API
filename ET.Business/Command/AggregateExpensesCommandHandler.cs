using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Business.ScheduledJobs;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ET.Business.Command
{
    public class AggregateExpensesCommandHandler :
        IRequestHandler<AggregateDailyExpensesCommand, ApiResponse<object>>,
        IRequestHandler<AggregateWeeklyExpensesCommand, ApiResponse<object>>,
        IRequestHandler<AggregateMonthlyExpensesCommand, ApiResponse<object>>
    {
        private readonly IScheduledJobService jobService;

        public AggregateExpensesCommandHandler(IScheduledJobService jobService)
        {
            this.jobService = jobService;
        }

        public async Task<ApiResponse<object>> Handle(AggregateDailyExpensesCommand request, CancellationToken cancellationToken)
        {
            return await jobService.AggregateDailyExpenses();
        }

        public async Task<ApiResponse<object>> Handle(AggregateWeeklyExpensesCommand request, CancellationToken cancellationToken)
        {
            return await jobService.AggregateWeeklyExpenses();
        }

        public async Task<ApiResponse<object>> Handle(AggregateMonthlyExpensesCommand request, CancellationToken cancellationToken)
        {
            return await jobService.AggregateMonthlyExpenses();
        }
    }
}
