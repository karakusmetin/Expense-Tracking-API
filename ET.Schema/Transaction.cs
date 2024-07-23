namespace ET.Schema
{
    public class TransactionRequest
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid UserId { get; set; }
    }
    public class TransactionResponse
    {
        public Guid TransactionId { get; set;}
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class TotalTransactionResponse
    {
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime TotalAmountDate { get; set; }
    }
}
