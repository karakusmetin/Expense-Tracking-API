using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ET.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator mediator;

        public TransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<TransactionResponse>>> Get()
        {
            var operation = new GetAllTransactionQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "admin,user")]

        public async Task<ApiResponse<TransactionResponse>> Get(Guid id)
        {
            var operation = new GetTransactioByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetTotalByUserId/{userId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ApiResponse<TotalTransactionResponse>> GetTotalByUserId(Guid userId)
        {
            var operation = new GetTotelTransactionsAmountByIdQuery(userId);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<ApiResponse<TransactionResponse>> Post([FromBody] TransactionRequest ApplicationUser)
        {
            var operation = new CreateTransactionCommand(ApplicationUser);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ApiResponse> Put(Guid id, [FromBody] TransactionRequest ApplicationUser)
        {
            var operation = new UpdateTransactionCommand(id, ApplicationUser);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ApiResponse> Delete(Guid id)
        {
            var operation = new DeleteTransactionCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
