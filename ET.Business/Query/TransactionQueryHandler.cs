using AutoMapper;
using ET.Base.Response;
using ET.Business.Cqrs;
using ET.Data;
using ET.Data.Entities;
using ET.Schema;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ET.Business.Query
{
    public class TransactionQueryHandler :
        IRequestHandler<GetAllTransactionQuery, ApiResponse<List<TransactionResponse>>>,
        IRequestHandler<GetTransactioByIdQuery, ApiResponse<TransactionResponse>>,
        IRequestHandler<GetTotelTransactionsAmountByIdQuery, ApiResponse<TotalTransactionResponse>>
    {
        private readonly ETDbContext dbContext;
        private readonly IMapper mapper;

        public TransactionQueryHandler(ETDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<TransactionResponse>>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var list = await dbContext.Set<Transaction>().ToListAsync(cancellationToken);
                var mappedList = mapper.Map<List<Transaction>, List<TransactionResponse>>(list);

                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse<List<TransactionResponse>>(mappedList);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<List<TransactionResponse>>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TransactionResponse>> Handle(GetTransactioByIdQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var entity = await dbContext.Set<Transaction>()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    return new ApiResponse<TransactionResponse>("Record not found");
                }

                var mapped = mapper.Map<Transaction, TransactionResponse>(entity);

                await transaction.CommitAsync(cancellationToken);

                return new ApiResponse<TransactionResponse>(mapped);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<TransactionResponse>($"An error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TotalTransactionResponse>> Handle(GetTotelTransactionsAmountByIdQuery request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var chechkUser = await dbContext.Set<ApplicationUser>()
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if(chechkUser == null)
                {
                    return new ApiResponse<TotalTransactionResponse>("User not found");
                }

                var list = await dbContext.Set<Transaction>().Where(x=>x.UserId == request.Id).ToListAsync(cancellationToken);
                var apiResponse = new ApiResponse<TotalTransactionResponse>("Total Amount");
                apiResponse.Response = new TotalTransactionResponse();
                apiResponse.Response.TotalAmountDate = DateTime.Now;
                apiResponse.Response.UserId = request.Id;
                apiResponse.Success = true;
                foreach (var item in list)
                {
                    apiResponse.Response.TotalAmount += item.Amount;
                }
                return apiResponse;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<TotalTransactionResponse>($"An error occurred: {ex.Message}");
            }

        }
    }
}