namespace NotUserEnPhone.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		//不要问为什么，必须这样..
		await Shell.Current.GoToAsync("//MainPage");
    }
}