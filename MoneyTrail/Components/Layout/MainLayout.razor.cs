namespace MoneyTrail.Components.Layout
{
    public partial class MainLayout
    {
        private bool isLoginPage => Nav.Uri.EndsWith("/login", StringComparison.OrdinalIgnoreCase);

    }
}