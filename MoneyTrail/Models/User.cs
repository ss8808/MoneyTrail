namespace MoneyTrail.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PreferredCurrency { get; set; }
    }
}
