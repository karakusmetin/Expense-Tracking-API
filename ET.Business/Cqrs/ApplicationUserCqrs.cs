using ET.Base.Response;
using ET.Schema;
using MediatR;

namespace ET.Business.Cqrs
{
    public record CreateApplicationUserCommand(ApplicationUserRequest Model) : IRequest<ApiResponse<ApplicationUserResponse>>;
    public record UpdateApplicationUserCommand(Guid Id, ApplicationUserRequest Model) : IRequest<ApiResponse>;
    public record DeleteApplicationUserCommand(Guid Id) : IRequest<ApiResponse>;

    public record GetAllApplicationUserQuery() : IRequest<ApiResponse<List<ApplicationUserResponse>>>;
    public record GetApplicationUserByIdQuery(Guid Id) : IRequest<ApiResponse<ApplicationUserResponse>>;
}
