using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tailuopai.Model
{
    public class WaitModel : ObservableObject
    {
        private string imageshebei1;
        public string ImageShebei1 { get => imageshebei1; set { SetProperty(ref imageshebei1, value); } }

        private string imageshebei2;
        public string ImageShebei2 { get => imageshebei2; set { SetProperty(ref imageshebei2, value); } }

        private string imageshebei3;
        public string ImageShebei3 { get => imageshebei3; set { SetProperty(ref imageshebei3, value); } }

        private int imageScaleY1 = 1;
        public int ImageScaleY1 { get => imageScaleY1; set { SetProperty(ref imageScaleY1, value); } }

        private int imageScaleY2 = 1;
        public int ImageScaleY2 { get => imageScaleY2; set { SetProperty(ref imageScaleY2, value); } }

        private int imageScaleY3 = 1;
        public int ImageScaleY3 { get => imageScaleY3; set { SetProperty(ref imageScaleY3, value); } }


        private string describe1 = string.Empty;
        public string Describe1 { get => describe1; set { SetProperty(ref describe1, value); } }

        private string describe2 = string.Empty;
        public string Describe2 { get => describe2; set { SetProperty(ref describe2, value); } }

        private string describe3 = string.Empty;
        public string Describe3 { get => describe3; set { SetProperty(ref describe3, value); } }

        private string anwser = string.Empty;
        public string Anwser { get => anwser; set { SetProperty(ref anwser, value); } }

        public ICommand BtnCommandOver { get; set; }

        public WaitModel()
        {
            BtnCommandOver = new RelayCommand<object>(DoBtnCommandOver);
        }

        private async void DoBtnCommandOver(object button)
        {
            Application.Current.MainPage.Navigation.PopModalAsync(); // 如果是使用PushModalAsync显示的话
        }


    }
}
