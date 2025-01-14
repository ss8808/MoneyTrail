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
        private decimal TotalTransaction { get; set; }
        private int TotalTransactionsCount { get; set; }
        private Transaction HighestInflow { get; set; }
        private Transaction HighestOutflow { get; set; }
        private Transaction HighestDebt { get; set; }

        private List<Transaction> Transactions = new();
        private List<Transaction> PendingDebts = new();

        private DateTime? _startDateTopTransactions;
        private DateTime? _endDateTopTransactions;

        private DateTime? _startDatePendingDebts;
        private DateTime? _endDatePendingDebts;

        private DateTime? StartDateTopTransactions
        {
            get => _startDateTopTransactions;
            set
            {
                if (_startDateTopTransactions != value)
                {
                    _startDateTopTransactions = value;
                    UpdateTopTransactionsChart();
                }
            }
        }

        private DateTime? EndDateTopTransactions
        {
            get => _endDateTopTransactions;
            set
            {
                if (_endDateTopTransactions != value)
                {
                    _endDateTopTransactions = value;
                    UpdateTopTransactionsChart();
                }
            }
        }

        private DateTime? StartDatePendingDebts
        {
            get => _startDatePendingDebts;
            set
            {
                if (_startDatePendingDebts != value)
                {
                    _startDatePendingDebts = value;
                    UpdatePendingDebtsChart();
                }
            }
        }

        private DateTime? EndDatePendingDebts
        {
            get => _endDatePendingDebts;
            set
            {
                if (_endDatePendingDebts != value)
                {
                    _endDatePendingDebts = value;
                    UpdatePendingDebtsChart();
                }
            }
        }

        private IEnumerable<Transaction> FilteredTopTransactions =>
            ApplyFilters(Transactions, StartDateTopTransactions, EndDateTopTransactions)
                .OrderByDescending(t => t.Amount)
                .Take(5);

        private IEnumerable<Transaction> FilteredPendingDebts =>
            ApplyFiltersByDueDate(PendingDebts, StartDatePendingDebts, EndDatePendingDebts);

        public List<ChartSeries> TopTransactionsChartSeries { get; set; } = new();
        public string[] TopTransactionsXAxisLabels { get; set; } = Array.Empty<string>();

        public List<ChartSeries> PendingDebtsChartSeries { get; set; } = new();
        public string[] PendingDebtsXAxisLabels { get; set; } = Array.Empty<string>();

        protected override async Task OnInitializedAsync()
        {
            var transactions = (await TransactionService.GetAllAsync()).ToList();

            // Calculate totals
            TotalInflows = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount);
            TotalOutflows = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);
            TotalDebts = transactions.Where(t => t.Type == TransactionType.Debt).Sum(t => t.Amount);
            PendingDebt = transactions.Where(t => t.Type == TransactionType.Debt && !t.IsCleared).Sum(t => t.Amount);
            ClearedDebts = transactions.Where(t => t.Type == TransactionType.Debt && t.IsCleared).Sum(t => t.Amount);

            TotalTransactionsCount = transactions.Count;
            TotalTransaction = TotalInflows + TotalDebts - TotalOutflows;

            // Calculate Highest Inflow, Outflow, and Debt Transactions
            HighestInflow = transactions.Where(t => t.Type == TransactionType.Credit)
                                        .OrderByDescending(t => t.Amount)
                                        .FirstOrDefault();

            HighestOutflow = transactions.Where(t => t.Type == TransactionType.Debit)
                                         .OrderByDescending(t => t.Amount)
                                         .FirstOrDefault();

            HighestDebt = transactions.Where(t => t.Type == TransactionType.Debt)
                                      .OrderByDescending(t => t.Amount)
                                      .FirstOrDefault();

            Transactions = transactions;
            PendingDebts = transactions.Where(t => t.Type == TransactionType.Debt && !t.IsCleared).ToList();

            UpdateTopTransactionsChart();
            UpdatePendingDebtsChart();
        }

        private IEnumerable<Transaction> ApplyFilters(IEnumerable<Transaction> transactions, DateTime? startDate, DateTime? endDate)
        {
            var filtered = transactions.AsEnumerable();

            if (startDate.HasValue)
            {
                filtered = filtered.Where(t => t.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                filtered = filtered.Where(t => t.Date <= endDate.Value);
            }

            return filtered;
        }

        private IEnumerable<Transaction> ApplyFiltersByDueDate(IEnumerable<Transaction> debts, DateTime? startDate, DateTime? endDate)
        {
            var filtered = debts.AsEnumerable();

            if (startDate.HasValue)
            {
                filtered = filtered.Where(t => t.DueDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                filtered = filtered.Where(t => t.DueDate <= endDate.Value);
            }
            return filtered;
        }

            private void UpdateTopTransactionsChart()
        {
            var filteredTransactions = FilteredTopTransactions.ToList();
            TopTransactionsXAxisLabels = filteredTransactions.Select(t => t.Title).ToArray();
            TopTransactionsChartSeries = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Top Transactions",
                    Data = filteredTransactions.Select(t => (double)t.Amount).ToArray()
                }
            };
        }

        private void UpdatePendingDebtsChart()
        {
            var filteredDebts = FilteredPendingDebts.ToList();
            PendingDebtsXAxisLabels = filteredDebts.Select(t => t.Title).ToArray();
            PendingDebtsChartSeries = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Pending Debts",
                    Data = filteredDebts.Select(t => (double)t.Amount).ToArray()
                }
            };
        }
    }
}
