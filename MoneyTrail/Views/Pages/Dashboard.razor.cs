using MoneyTrail.Enums;
using MoneyTrail.Models;
using MudBlazor;


namespace MoneyTrail.Views.Pages
{
    public partial class Dashboard
    {
        private decimal TotalInflows { get; set; }
        private decimal TotalOutflows { get; set; }
        private decimal TotalDebts { get; set; }
        private decimal PendingDebt { get; set; }
        private decimal ClearedDebts { get; set; }

        private List<Transaction> Transactions = new();

        private List<Transaction> TopTransactions { get; set; } = new();
        private List<Transaction> PendingDebts { get; set; } = new();
        private DateTime? StartDate { get; set; }
        private DateTime? EndDate { get; set; }

        // Filtered Pending Debts
        private IEnumerable<Transaction> FilteredPendingDebts => ApplyFilters();

        private List<KeyValuePair<string, decimal>> ChartData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var transactions = (await TransactionService.GetAllAsync()).ToList();

            // Calculate totals
            TotalInflows = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount);
            TotalOutflows = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);
            TotalDebts = transactions.Where(t => t.Type == TransactionType.Debt).Sum(t => t.Amount);
            PendingDebt = transactions.Where(t => t.Type == TransactionType.Debt && !t.IsCleared).Sum(t => t.Amount);
            ClearedDebts = transactions.Where(t => t.Type == TransactionType.Debt && t.IsCleared).Sum(t => t.Amount);

            // Get the top 5 highest transactions
            TopTransactions = transactions.OrderByDescending(t => t.Amount).Take(5).ToList();

            // Filter and get pending debts
            PendingDebts = transactions.Where(t => t.Type == TransactionType.Debt && !t.IsCleared).ToList();

            Series = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Financial Statistics",
                    Data = new double[]
                    {
                        (double)TotalInflows,
                        (double)TotalOutflows,
                        (double)TotalDebts,
                        (double)PendingDebt,
                        (double)ClearedDebts
                    }
                }
            };

        }

        private IEnumerable<Transaction> ApplyFilters()
        {
            var filtered = PendingDebts.AsEnumerable();

            if (StartDate.HasValue)
            {
                filtered = filtered.Where(t => t.DueDate >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                filtered = filtered.Where(t => t.DueDate <= EndDate.Value);
            }

            return filtered;
        }

        private int Index = -1; // Default value cannot be 0 -> first selected index is 0.

        public List<ChartSeries> Series = new List<ChartSeries>();
        public string[] XAxisLabels = { "Total Inflows", "Total Outflows", "Total Debts", "Pending Debts", "Cleared Debts" };

    }
}