using ET.Base.Response;
using ET.Schema;
using MediatR;


namespace ET.Business.Cqrs
{
    public record CreateTokenCommand(TokenRequest Model) : IRequest<ApiResponse<TokenResponse>>;
}
