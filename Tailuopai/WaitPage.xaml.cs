using Tailuopai.Model;
using static Android.Graphics.ColorSpace;

namespace Tailuopai;

public partial class WaitPage : ContentPage
{
    public WaitModel model { get; set; } = new WaitModel();
    public WaitPage()
    {
		InitializeComponent();
        this.BindingContext = model;
    }
}