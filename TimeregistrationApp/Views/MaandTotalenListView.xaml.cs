using TimeregistrationApp.ViewModels;

namespace TimeregistrationApp.Views;

public partial class MaandTotalenListView : ContentPage
{
	public MaandTotalenListView(MaandTotalenListViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}