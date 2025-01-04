using MoneyTrail.Data;

namespace MoneyTrail.Services
{
    public class AuthService
    {
        private readonly InMemory _db;
        public AuthService(InMemory db) => _db = db;

        public bool Login(string username, string password)
        {
            return _db.Users.Any(u => u.Username == username && u.PasswordHash == password);
        }
    }
}
