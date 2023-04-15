using TimeregistrationApp.ViewModels;

namespace TimeregistrationApp;

public partial class MainPage : ContentPage
{

	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

