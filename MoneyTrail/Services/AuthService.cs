using System.Text.Json;
using MoneyTrail.Data;
using MoneyTrail.Models;

namespace MoneyTrail.Services
{
    public class AuthService
    {
        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "users.json");
        public string SelectedCurrency { get; private set; } = "USD";
        private List<User> _users;

        public AuthService()
        {
            _users = LoadUsers();
        }

        // Login method
        public bool Login(string username, string password, string preferredCurrency)
        {
            // Hardcoded admin credentials
            if (username == "admin" && password == "admin123")
            {
                SelectedCurrency = preferredCurrency; // Allow preferred currency change for admin
                return true;
            }

            // Check the file-based user data for other users
            var user = _users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.PasswordHash != password)
            {
                return false; // Invalid credentials
            }

            // Update preferred currency
            user.PreferredCurrency = preferredCurrency;
            SelectedCurrency = preferredCurrency;

            SaveUsers(_users); // Persist the updated user data
            return true;
        }

        // Get the preferred currency of the logged-in user
        public string GetPreferredCurrency()
        {
            return SelectedCurrency;
        }

        // Load users from the JSON file or use SeedData if the file doesn't exist
        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
            {
                // Use SeedData and save to file if the file doesn't exist
                var seedUsers = SeedData.SeedUsers();
                SaveUsers(seedUsers);
                return seedUsers;
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        // Save users to the JSON file
        private void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
