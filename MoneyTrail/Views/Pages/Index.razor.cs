using Microsoft.AspNetCore.Components;

namespace MoneyTrail.Views.Pages
{
    public partial class Index: ComponentBase
    {
        protected override void OnInitialized()
        {
            Nav.NavigateTo("/login");
        }
    }
}