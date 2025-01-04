using Microsoft.AspNetCore.Components;
using MoneyTrail.Models;
using MoneyTrail.Services;

namespace MoneyTrail.Views.Pages
{
    public partial class Login
    {
        private string? ErrorMessage;

        public User user { get; set; } = new();

        [Inject]
        private AuthService authService { get; set; }

        private async Task HandleLogin()
        {
            if (AuthService.Login(user.Username, user.PasswordHash))
            {
                Nav.NavigateTo("/dashboard");
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }

    }
}