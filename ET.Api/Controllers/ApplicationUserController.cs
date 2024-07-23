using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Schema;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ET.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApplicationUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllApplicationUsers/")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<ApplicationUserResponse>>> Get()
        {
            var operation = new GetAllApplicationUserQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("GetByApplicationUserId/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ApplicationUserResponse>> Get(Guid id)
        {
            var operation = new GetApplicationUserByIdQuery(id);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<ApplicationUserResponse>> Post([FromBody] ApplicationUserRequest ApplicationUser)
        {
            var operation = new CreateApplicationUserCommand(ApplicationUser);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("PutByApplicationUserId/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Put(Guid id, [FromBody] ApplicationUserRequest ApplicationUser)
        {
            var operation = new UpdateApplicationUserCommand(id, ApplicationUser);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("DeleteByApplicationUserId/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(Guid id)
        {
            var operation = new DeleteApplicationUserCommand(id);
            var result = await mediator.Send(operation);
            return result;
        }
    }

}

