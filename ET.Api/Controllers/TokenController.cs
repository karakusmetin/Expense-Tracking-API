﻿using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Schema;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ET.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IMediator mediator;

        public TokenController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResponse<TokenResponse>> Post([FromBody] TokenRequest request)
        {
            var operation = new CreateTokenCommand(request);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
