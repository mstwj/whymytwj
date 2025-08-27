using CommunityToolkit.Maui.Views;
using MyIPHoneNonChang.ViewModels.Dialog;

namespace MyIPHoneNonChang.Views.Dialog;

public partial class SettingsView : Popup
{
	public SettingsView(SettingsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}