using TimeregistrationApp.ViewModels;

namespace TimeregistrationApp.Views;

public partial class TijdRegistratieDetailView : ContentPage
{
	public TijdRegistratieDetailView(TijdRegistratieDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}