using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Views.Pages
{
    public partial class Dashboard
    {
        private decimal TotalInflows { get; set; }
        private decimal TotalOutflows { get; set; }
        private decimal TotalDebts { get; set; }
        private List<Transaction> TopTransactions { get; set; } = new();
        private List<Transaction> PendingDebts { get; set; } = new();
        private DateTime? StartDate { get; set; }
        private DateTime? EndDate { get; set; }

        private List<KeyValuePair<string, decimal>> ChartData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var transactions = (await TransactionService.GetAllAsync()).ToList();

            // Calculate totals
            TotalInflows = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount);
            TotalOutflows = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);
            TotalDebts = transactions.Where(t => t.Type == TransactionType.Debt).Sum(t => t.Amount);

            // Get the top 5 highest transactions
            TopTransactions = transactions.OrderByDescending(t => t.Amount).Take(5).ToList();

            // Filter and get pending debts
            PendingDebts = transactions.Where(t => t.Type == TransactionType.Debt && !t.IsCleared &&
                                                    (StartDate == null || t.Date >= StartDate) &&
                                                    (EndDate == null || t.Date <= EndDate)).ToList();

            // Prepare data for the graph
            ChartData = new List<KeyValuePair<string, decimal>>()
        {
            new KeyValuePair<string, decimal>("Inflows", TotalInflows),
            new KeyValuePair<string, decimal>("Outflows", TotalOutflows),
            new KeyValuePair<string, decimal>("Debts", TotalDebts)
        };
        }

    }
}