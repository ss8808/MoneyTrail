using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Views.Pages
{
    public partial class CreditList
    {

        
        private string TagsInput = "";
        private string SearchText = "";
        private DateTime? StartDate;
        private DateTime? EndDate;
        private string? SelectedTransactionType;
        private string SortOrder = "Ascending";
        private List<Transaction> Transactions = new();
        private IEnumerable<Transaction> FilteredTransactions => ApplyFilters();
        private IEnumerable<Transaction> CreditTransactions => FilteredTransactions
        .Where(t => t.Type == TransactionType.Credit)
        .OrderBy(t => SortOrder == "Ascending" ? t.Date : DateTime.MinValue)
        .ThenByDescending(t => SortOrder == "Descending" ? t.Date : DateTime.MinValue);


        protected override async Task OnInitializedAsync()
        {
            var allTransactions = await TransactionService.GetAllAsync();
            Transactions = allTransactions
                .Where(t => t.Type == TransactionType.Credit)
                .ToList();
            

        }

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
