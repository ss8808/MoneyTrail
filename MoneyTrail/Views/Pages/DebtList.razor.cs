using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Views.Pages
{
    public partial class DebtList
    {
        private List<Transaction> DebtTransactions = new();

        protected override async Task OnInitializedAsync()
        {
            var allTransactions = await TransactionService.GetAllAsync();
            DebtTransactions = allTransactions
                .Where(t => t.Type == TransactionType.Debt)
                .ToList();
        }

    }
}