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
    public class TransactionCommandHandler : 
        IRequestHandler<CreateTransactionCommand, ApiResponse<TransactionResponse>>,
        IRequestHandler<UpdateTransactionCommand, ApiResponse>,
        IRequestHandler<DeleteTransactionCommand, ApiResponse>

    {
        private readonly ETDbContext dbContext;
        private readonly IMapper mapper;

        public TransactionCommandHandler(ETDbContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<TransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var checkUser = await dbContext.ApplicationUsers
                    .Where(x => x.Id == request.Model.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (checkUser == null)
                {
                    return new ApiResponse<TransactionResponse>($"{request.Model.UserId} user not found.");
                }
                var checkTransaction = await dbContext.Transactions.Where(x=>x.Id == request.Model.TransactionId).FirstOrDefaultAsync(cancellationToken);
                if (checkTransaction != null)
                {
       
                    return new ApiResponse<TransactionResponse>("Transaction already have");
                }
                var entity = mapper.Map<TransactionRequest, Transaction>(request.Model);
                entity.InsertDate = DateTime.UtcNow;


                var entityResult = await dbContext.Transactions.AddAsync(entity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var mapped = mapper.Map<Transaction, TransactionResponse>(entityResult.Entity);
                return new ApiResponse<TransactionResponse>(mapped);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ApiResponse<TransactionResponse>($"An error occurred: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var fromdb = await dbContext.Set<Transaction>()
                    .Where(x => x.Id == request.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromdb == null)
                {
                    return new ApiResponse("Record not found");
                }
                
                fromdb.UpdateDate = DateTime.UtcNow;
                fromdb.Amount = request.Model.Amount;
                fromdb.Description = request.Model.Description;
                

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

        public async Task<ApiResponse> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var fromdb = await dbContext.Set<Transaction>()
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
