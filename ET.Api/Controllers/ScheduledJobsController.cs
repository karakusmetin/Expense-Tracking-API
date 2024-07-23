using ET.Business.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ET.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledJobsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScheduledJobsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("aggregate-daily-expenses")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AggregateDailyExpenses()
        {
            var result = await mediator.Send(new AggregateDailyExpensesCommand());
            return Ok(result);
        }

        [HttpPost("aggregate-weekly-expenses")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AggregateWeeklyExpenses()
        {
            var result = await mediator.Send(new AggregateWeeklyExpensesCommand());
            return Ok(result);
        }

        [HttpPost("aggregate-monthly-expenses")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AggregateMonthlyExpenses()
        {
            var result = await mediator.Send(new AggregateMonthlyExpensesCommand());
            return Ok(result);
        }
    }
}
