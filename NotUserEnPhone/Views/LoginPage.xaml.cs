namespace NotUserEnPhone.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		//��Ҫ��Ϊʲô����������..
		await Shell.Current.GoToAsync("//MainPage");
    }
}