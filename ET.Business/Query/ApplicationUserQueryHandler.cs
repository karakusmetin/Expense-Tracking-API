using AutoMapper;
using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Data;
using ET.Data.Entities;
using ET.Schema;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ET.Business.Query
{
    public class ApplicationUserQueryHandler :
        IRequestHandler<GetAllApplicationUserQuery, ApiResponse<List<ApplicationUserResponse>>>,
        IRequestHandler<GetApplicationUserByIdQuery, ApiResponse<ApplicationUserResponse>>
    {
        private readonly ETDbContext dbContext;
        private readonly IMapper mapper;

        public ApplicationUserQueryHandler(ETDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<ApplicationUserResponse>>> Handle(GetAllApplicationUserQuery request,
            CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var list = await dbContext.Set<ApplicationUser>().ToListAsync(cancellationToken);
                var mappedList = mapper.Map<List<ApplicationUser>, List<ApplicationUserResponse>>(list);

                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse<List<ApplicationUserResponse>>(mappedList);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<List<ApplicationUserResponse>>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ApplicationUserResponse>> Handle(GetApplicationUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var entity = await dbContext.Set<ApplicationUser>()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    return new ApiResponse<ApplicationUserResponse>("Record not found");
                }

                var mapped = mapper.Map<ApplicationUser, ApplicationUserResponse>(entity);

                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse<ApplicationUserResponse>(mapped);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<ApplicationUserResponse>($"An error occurred: {ex.Message}");
            }
        }
    }
}
