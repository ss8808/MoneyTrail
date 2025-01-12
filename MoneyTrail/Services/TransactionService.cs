using System.Text.Json;
using MoneyTrail.Enums;
using MoneyTrail.Models;

namespace MoneyTrail.Services
{
    public class TransactionService
    {
        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
        private List<Transaction> _transactions = new();

        public TransactionService()
        {
            // Load transactions from the JSON file
            _transactions = LoadTransactions();
        }

        // Retrieve all transactions asynchronously
        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            await Task.Delay(100); // Simulate asynchronous operation
            return _transactions;
        }

        // Retrieve a transaction by ID asynchronously
        public async Task<Transaction?> GetByIdAsync(int id)
        {
            await Task.Delay(100); // Simulate asynchronous operation
            return _transactions.FirstOrDefault(t => t.Id == id);
        }

        // Add a new transaction asynchronously
        public async Task AddAsync(Transaction transaction)
        {
            await Task.Delay(100); // Simulate asynchronous operation

            transaction.Id = _transactions.Any() ? _transactions.Max(t => t.Id) + 1 : 1;
            _transactions.Add(transaction);

            // Save updated transactions to the JSON file
            SaveTransactions(_transactions);
        }

        // Update an existing transaction asynchronously
        public async Task UpdateAsync(Transaction transaction)
        {
            await Task.Delay(100); // Simulate asynchronous operation

            var existing = await GetByIdAsync(transaction.Id);
            if (existing != null)
            {
                existing.Title = transaction.Title;
                existing.Type = transaction.Type;
                existing.Amount = transaction.Amount;
                existing.Date = transaction.Date;
                existing.Tags = transaction.Tags;

                // Save updated transactions to the JSON file
                SaveTransactions(_transactions);
            }
        }

        // Delete a transaction asynchronously
        public async Task DeleteAsync(int id)
        {
            await Task.Delay(100); // Simulate asynchronous operation

            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _transactions.Remove(transaction);

                // Save updated transactions to the JSON file
                SaveTransactions(_transactions);
            }
        }

        // Load transactions from the JSON file
        private List<Transaction> LoadTransactions()
        {
            if (!File.Exists(FilePath)) return new List<Transaction>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        // Save transactions to the JSON file
        private void SaveTransactions(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
