namespace MoneyTrail.Components.Layout
{
    public partial class NavMenu
    {
        private bool isLoginPage => Nav.Uri.EndsWith("/login", StringComparison.OrdinalIgnoreCase);
    }
}