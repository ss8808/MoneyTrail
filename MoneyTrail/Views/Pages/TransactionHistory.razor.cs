using MoneyTrail.Enums;
using MoneyTrail.Models;
using MudBlazor;

namespace MoneyTrail.Views.Pages
{
    public partial class TransactionHistory
    {
        private List<Transaction> Transactions = new();
        private List<Transaction> FilteredTransactions { get; set; } = new List<Transaction>();
        private List<Transaction> TransactionsDetails { get; set; } = new List<Transaction>();


        private decimal Balance { get; set; }
        private Transaction NewTransaction = new();
        private Transaction? EditTransactionModel;
        private string TagsInput = "";
        private string SearchText = "";
        private DateTime? StartDate;
        private DateTime? EndDate;
        private string? SelectedTransactionType;
        private string? Message {  get; set; }

        private bool IsAddFormVisible = false;
        private bool IsEditFormVisible = false;
        

        private List<string> AllTags { get; set; } = new();
        private string SelectedTag { get; set; } = string.Empty;

        private List<string> PredefinedTags { get; set; } = new()
        {
            "Yearly", "Monthly", "Food", "Drinks", "Clothes",
            "Gadgets", "Miscellaneous", "Fuel", "Rent", "EMI", "Party"
        };



        protected override async Task OnInitializedAsync()
        {
            // Fetch the transactions, and filter out any null transactions
            Transactions = (await TransactionService.GetAllAsync())
                            .Where(t => t != null)  // Filter out null transactions
                            .ToList();

            var transactionTags = Transactions.SelectMany(t => t.Tags ?? new List<string>()).Distinct();
            AllTags = PredefinedTags.Union(transactionTags).OrderBy(tag => tag).ToList();

            UpdateBalance();
           

        }


        private void FilterTransactions()
        {
            FilteredTransactions = Transactions
                .Where(t => (string.IsNullOrEmpty(SearchText) || t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                            && (string.IsNullOrEmpty(SelectedTransactionType) || t.Type.ToString() == SelectedTransactionType)
                            && (StartDate == null || t.Date >= StartDate)
                            && (EndDate == null || t.Date <= EndDate)
                            && (string.IsNullOrEmpty(SelectedTag) || t.Tags.Contains(SelectedTag))).ToList();
            

        }


        private void OpenAddForm()
        {
            NewTransaction = new Transaction(); // Reset new transaction
            TagsInput = string.Empty; // Reset Tags input
            IsAddFormVisible = true;
            IsEditFormVisible = false;
            StateHasChanged();
        }

        private void AddPredefinedTag(string tag)
        {
            var tags = TagsInput.Split(',').Select(t => t.Trim()).ToList();
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
                TagsInput = string.Join(", ", tags);
            }
            if (!string.IsNullOrEmpty(TagsInput))
            {
                NewTransaction.Tags = TagsInput.Split(',')
                                                .Select(tag => tag.Trim())
                                                .Where(tag => !string.IsNullOrEmpty(tag))
                                                .Distinct()
                                                .ToList();
            }
        }

        



    private void EditTransaction(Transaction transaction)
        {
            EditTransactionModel = transaction;
            TagsInput = string.Join(",", transaction.Tags);
            IsAddFormVisible = false;
            IsEditFormVisible = true;
            StateHasChanged();
        }

        private void CloseAddForm()
        {
            IsAddFormVisible = false;
            StateHasChanged();
        }

        private void CloseEditForm()
        {
            IsEditFormVisible = false;
            StateHasChanged();
        }

        private async Task OnAddTransaction()
        {
            Message = null;
            // Parse the comma-separated tags
            if (!string.IsNullOrEmpty(TagsInput))
            {
                NewTransaction.Tags = TagsInput.Split(',').Select(tag => tag.Trim()).ToList();
            }

           

            // Handle Debt-specific fields
            

            // If it's a cleared debt, remove the debt amount from the balance (as it's paid)
            if (NewTransaction.Type == TransactionType.Debt)
            {
                Balance = NewTransaction.Amount; // If it's cleared, treat it as cash inflow
            }
            
            else if (NewTransaction.Type == TransactionType.Credit)
            {
                Balance += NewTransaction.Amount;
            }
            else if (NewTransaction.Type == TransactionType.Debit)
            {
                if (Balance >= NewTransaction.Amount)
                {
                    Balance -= NewTransaction.Amount;
                }
                else
                {
                    // Show balance insufficient message
                    Message = "Insufficient balance.";
                    return;
                }
            }

            await TransactionService.AddAsync(NewTransaction);
            Transactions = (await TransactionService.GetAllAsync()).ToList(); // Refresh the list
            UpdateBalance();
            CloseAddForm();
            Snackbar.Add("Transaction added successfully.", Severity.Success);
        }

        private async Task OnEditTransaction()
        {
            // Parse the comma-separated tags
            if (!string.IsNullOrEmpty(TagsInput))
            {
                EditTransactionModel!.Tags = TagsInput.Split(',').Select(tag => tag.Trim()).ToList();
            }

            // Handle Debt-specific fields
            

            // If it's a cleared debt, adjust balance
            if (EditTransactionModel.Type == TransactionType.Debt && EditTransactionModel.IsCleared)
            {
                if (Balance >= EditTransactionModel.Amount)
                {
                    Balance -= EditTransactionModel.Amount;
                }
                else
                {
                    // Show balance insufficient message
                    Message = "Insufficient balance.";
                    return;
                } 
            }
            else if(EditTransactionModel.Type == TransactionType.Debt && !(EditTransactionModel.IsCleared))
            {
                Balance = EditTransactionModel.Amount;
            }
            else if (EditTransactionModel.Type == TransactionType.Credit)
            {
                Balance += EditTransactionModel.Amount;
            }
            else if (EditTransactionModel.Type == TransactionType.Debit)
            {
                if (Balance >= EditTransactionModel.Amount)
                {
                    Balance -= EditTransactionModel.Amount;
                }
                else
                {
                    // Show balance insufficient message
                    Message = "Insufficient balance.";
                    return;
                }
            }

            await TransactionService.UpdateAsync(EditTransactionModel);
            Transactions = (await TransactionService.GetAllAsync()).ToList(); // Refresh the list
            UpdateBalance();
            CloseEditForm();
            Snackbar.Add("Transaction updated successfully.", Severity.Success);
        }

        private async Task DeleteTransaction(int id)
        {
            var transaction = await TransactionService.GetByIdAsync(id);

            if (transaction != null)
            {
                if (transaction.Type == TransactionType.Debit || transaction.Amount <= Balance
                    || (transaction.Type == TransactionType.Debt && transaction.IsCleared)
                )
                {
                    await TransactionService.DeleteAsync(id);
                    Transactions = (await TransactionService.GetAllAsync()).ToList();
                    UpdateBalance();
                    Snackbar.Add("Transaction deleted successfully.", Severity.Success);
                }
                else
                {
                    Snackbar.Add("The transaction amount exceeds the current balance and cannot be deleted.", Severity.Error);

                }
            }
   
        }


        private void UpdateBalance()
        {
            Balance = Transactions
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount)
                - Transactions
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount)
                - Transactions
                .Where(t => t.Type == TransactionType.Debt && t.IsCleared)  // Subtract cleared debt transactions
                .Sum(t => t.Amount);  // Subtract the debt amount for cleared transactions
        }
        

    }
}

    