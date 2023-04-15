using TimeregistrationApp.ViewModels;

namespace TimeregistrationApp.Views;

public partial class TijdRegistratieListView : ContentPage
{
	public TijdRegistratieListView(TijdRegistratieListViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}