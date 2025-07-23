using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp外观UI.ViewModel
{
    public class MainWindowViewModel : ObservableValidator
    {
        private object _contentView;
        public object ContentView { get => _contentView; set { SetProperty(ref _contentView, value); } }
        public ICommand BtnCommandStart { get; set; }

        public List<MenuModel> MenuList { get; set; } = new List<MenuModel>();

        public MainWindowViewModel()
        {
            MenuModel mm = new MenuModel();
            mm.Header = "NotifyIconControl";
            mm.Icon = "\ue610";
            mm.Obj = "NotifyIconControl";
            MenuList.Add(mm);

            BtnCommandStart = new RelayCommand<object>(DoBtnCommandStart);

        }

        private void DoBtnCommandStart(object param)
        {
            if (ContentView != null && ContentView.GetType().Name == param.ToString()) return;
            Type type = Assembly.GetExecutingAssembly().GetType("WpfApp外观UI.Control." + param.ToString())!;
            this.ContentView = Activator.CreateInstance(type)!;
        }
    }

    public class MenuModel
    {
        public string Icon { get; set; }
        public string Header { get; set; }

        public string Obj { get; set; }


    }

}
