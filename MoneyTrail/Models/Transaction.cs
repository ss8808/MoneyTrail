using MoneyTrail.Enums;

namespace MoneyTrail.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
    }
}
