using TimeregistrationApp.Views;

namespace TimeregistrationApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(TijdRegistratieDetailView), typeof(TijdRegistratieDetailView));
    }
}
