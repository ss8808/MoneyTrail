using MoneyTrail.Models;

namespace MoneyTrail.Data
{
    public static class SeedData
    {
        public static List<User> SeedUsers() => new()
        {
            new User {Username = "admin", PasswordHash = "admin123", PreferredCurrency = "NPR"}
        };
    }
}
