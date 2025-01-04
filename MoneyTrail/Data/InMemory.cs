using MoneyTrail.Models;

namespace MoneyTrail.Data
{
    public class InMemory
    {
        public List<User> Users { get; set; } = SeedData.SeedUsers();
    }
}
