namespace MoneyTrail.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCleared { get; set; }
    }
}
