using MoneyTrail.Enums;

namespace MoneyTrail.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public List<string> Tags { get; set; }
        public string? Note {  get; set; }

        //debt specific fields
        public bool IsCleared { get; set; }
        public string Source { get; set; }
        public DateTime DueDate { get; set; }
    }
}
