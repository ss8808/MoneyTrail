using Microsoft.AspNetCore.Components;
using MoneyTrail.Models;
using MoneyTrail.Services;
using MudBlazor;

namespace MoneyTrail.Views.Pages
{
    public partial class Login
    {
        private string? ErrorMessage;
        private MudForm? form;

        public User user { get; set; } = new();



        private async Task HandleLogin()
        {
            // Simulate a login process
            if (AuthService.Login(user.Username, user.PasswordHash, user.PreferredCurrency))
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