using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Views.Pages
{
    public partial class DebtList
    {
        private string SearchText = "";
        private DateTime? StartDate;
        private DateTime? EndDate;
        private string? SelectedTransactionType;
        private string SortOrder = "Ascending";
        private List<Transaction> Transactions = new();
        private IEnumerable<Transaction> FilteredTransactions => ApplyFilters();

        // Sorting logic: apply sorting by date (ascending or descending)
        private IEnumerable<Transaction> DebtTransactions => FilteredTransactions
        .Where(t => t.Type == TransactionType.Debt)
        .OrderBy(t => SortOrder == "Ascending" ? t.Date : DateTime.MinValue)
        .ThenByDescending(t => SortOrder == "Descending" ? t.Date : DateTime.MinValue);
        // On component initialization, fetch all debt transactions
        protected override async Task OnInitializedAsync()
        {
            var allTransactions = await TransactionService.GetAllAsync();
            Transactions = allTransactions
                .Where(t => t.Type == TransactionType.Debt)
                .ToList();
        }

        // Apply filters based on search text and date range
        private IEnumerable<Transaction> ApplyFilters()
        {
            var filtered = Transactions.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchText))
            {
                filtered = filtered.Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (StartDate.HasValue)
            {
                filtered = filtered.Where(t => t.Date >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                filtered = filtered.Where(t => t.Date <= EndDate.Value);
            }


            return filtered;
        }
    }
}
