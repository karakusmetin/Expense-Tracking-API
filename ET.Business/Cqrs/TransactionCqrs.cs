using ET.Base.Response;
using ET.Schema;
using MediatR;

namespace ET.Business.Cqrs
{
    public record CreateTransactionCommand(TransactionRequest Model) : IRequest<ApiResponse<TransactionResponse>>;
    public record UpdateTransactionCommand(Guid Id, TransactionRequest Model) : IRequest<ApiResponse>;
    public record DeleteTransactionCommand(Guid Id) : IRequest<ApiResponse>;

    public record GetAllTransactionQuery() : IRequest<ApiResponse<List<TransactionResponse>>>;
    public record GetTransactioByIdQuery(Guid Id) : IRequest<ApiResponse<TransactionResponse>>;
    public record GetTotelTransactionsAmountByIdQuery(Guid Id) : IRequest<ApiResponse<TotalTransactionResponse>>;
}
