using AutoMapper;
using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Data;
using ET.Data.Entities;
using ET.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ET.Business.Command
{
    public class ApplicationUserCommandHandler :
    IRequestHandler<CreateApplicationUserCommand, ApiResponse<ApplicationUserResponse>>,
    IRequestHandler<UpdateApplicationUserCommand, ApiResponse>,
    IRequestHandler<DeleteApplicationUserCommand, ApiResponse>
    {
        private readonly ETDbContext dbContext;
        private readonly IMapper mapper;

        public ApplicationUserCommandHandler(ETDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<ApplicationUserResponse>> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var checkIdentity = await dbContext.Set<ApplicationUser>()
                    .Where(x => x.UserName == request.Model.UserName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (checkIdentity != null)
                {
                    return new ApiResponse<ApplicationUserResponse>($"{request.Model.UserName} is in use.");
                }

                var entity = mapper.Map<ApplicationUserRequest, ApplicationUser>(request.Model);
                entity.InsertDate = DateTime.UtcNow;

                var entityResult = await dbContext.AddAsync(entity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var mapped = mapper.Map<ApplicationUser, ApplicationUserResponse>(entityResult.Entity);
                return new ApiResponse<ApplicationUserResponse>(mapped);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<ApplicationUserResponse>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var fromdb = await dbContext.Set<ApplicationUser>()
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromdb == null)
                {
                    return new ApiResponse("Record not found");
                }

                fromdb.FirstName = request.Model.FirstName;
                fromdb.LastName = request.Model.LastName;
                fromdb.Email = request.Model.Email;
                fromdb.Role = request.Model.Role;

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var fromdb = await dbContext.Set<ApplicationUser>()
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromdb == null)
                {
                    return new ApiResponse("Record not found");
                }

                fromdb.IsActive = false;
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}
