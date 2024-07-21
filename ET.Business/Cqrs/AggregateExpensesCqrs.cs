using ET.Base.Response;
using MediatR;

namespace ET.Business.Cqrs
{
    public class AggregateDailyExpensesCommand : IRequest<ApiResponse<object>> { }
    public class AggregateWeeklyExpensesCommand : IRequest<ApiResponse<object>> { }
    public class AggregateMonthlyExpensesCommand : IRequest<ApiResponse<object>> { }

}
