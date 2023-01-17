namespace Dsl.Domain.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }

        public string AccountNumber { get; set; }

        public decimal Amount { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Particular { get; set; }

        public DateTimeOffset ProccessedDate { get; set; }

        public string Reference { get; set; }

        public string Status { get; set; }

        public DateTimeOffset TransactionDate { get; set; }
    }
}