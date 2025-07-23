using Tailuopai.Model;

namespace Tailuopai
{
    public partial class MainPage : ContentPage
    {
        public MainViewModel model { get; set; } = new MainViewModel();        

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = model;
        }

    }

}
