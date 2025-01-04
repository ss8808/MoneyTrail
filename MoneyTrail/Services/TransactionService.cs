using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Services
{
    public class TransactionService
    {
        // In-memory data store (should be replaced with a database in production)
        private readonly List<Transaction> _transactions = new();

        public TransactionService()
        {
            // Optionally, add some mock data for testing
            if (!_transactions.Any())
            {
                _transactions.Add(new Transaction
                {
                    Id = 1,
                    Title = "Sample Credit",
                    Type = TransactionType.Credit,
                    Amount = 100.00m,
                    Date = DateTime.Now.AddDays(-10),
                    Tags = new List<string> { "Income" }
                });
                _transactions.Add(new Transaction
                {
                    Id = 2,
                    Title = "Sample Debit",
                    Type = TransactionType.Debit,
                    Amount = 50.00m,
                    Date = DateTime.Now.AddDays(-5),
                    Tags = new List<string> { "Expense" }
                });
            }
        }

        // Retrieve all transactions asynchronously
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            // Simulating an async operation (e.g., a database query)
            await Task.Delay(100);
            return _transactions;
        }

        // Retrieve a transaction by ID asynchronously
        public async Task<Transaction?> GetByIdAsync(int id)
        {
            await Task.Delay(100);
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

        // Add a new transaction asynchronously
        public async Task AddAsync(Transaction transaction)
        {
            // Simulate async database operation
            await Task.Delay(100);

            transaction.Id = _transactions.Any() ? _transactions.Max(t => t.Id) + 1 : 1;
            _transactions.Add(transaction);
        }

        // Update an existing transaction asynchronously
        public async Task UpdateAsync(Transaction transaction)
        {
            await Task.Delay(100);

            var existing = await GetByIdAsync(transaction.Id);
            if (existing != null)
            {
                existing.Title = transaction.Title;
                existing.Type = transaction.Type;
                existing.Amount = transaction.Amount;
                existing.Date = transaction.Date;
                existing.Tags = transaction.Tags;
            }
        }

        // Delete a transaction asynchronously
        public async Task DeleteAsync(int id)
        {
            await Task.Delay(100);

            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _transactions.Remove(transaction);
            }
        }
    }
}
