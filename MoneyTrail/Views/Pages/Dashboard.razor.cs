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

        public List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "United States", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new ChartSeries() { Name = "Germany", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new ChartSeries() { Name = "Sweden", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };

        public string[] XAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" };



    }
}