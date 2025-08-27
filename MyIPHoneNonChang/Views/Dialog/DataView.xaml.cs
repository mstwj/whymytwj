using CommunityToolkit.Maui.Views;
using MyIPHoneNonChang.ViewModels.Dialog;

namespace MyIPHoneNonChang.Views.Dialog;

public partial class DataView : Popup
{
	public DataView(DataViewModel dataViewModel)
	{
		InitializeComponent();
		this.BindingContext = dataViewModel;
	}
}