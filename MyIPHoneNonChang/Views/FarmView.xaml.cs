using MyIPHoneNonChang.ViewModels;

namespace MyIPHoneNonChang.Views;

public partial class FarmView : ContentPage
{
	public FarmView(FramViewModel framViewModel)
	{
		InitializeComponent();
		this.BindingContext = framViewModel;

    }
}