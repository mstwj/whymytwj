using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace 我的地图.View
{
    public class MainWindowModel : ObservableValidator
    {
        private object _contentView;
        public object ContentView { get => _contentView; set { SetProperty(ref _contentView, value); } }

        public ICommand MenuCommand { get; set; }
        public ObservableCollection<ColorModel> WHColors { get; set; } = new ObservableCollection<ColorModel>();

        public ObservableCollection<ColorModel> XAColors { get; set; } = new ObservableCollection<ColorModel>();

        public MainWindowModel()
        {

            MenuCommand = new RelayCommand<object>(DoMenuCommand);            

            for (int i = 0; i < 5; i++)
            {
                WHColors.Add(new ColorModel { Color = Brushes.OrangeRed });
            }

            for (int i = 0; i < 5; i++)
            {
                XAColors.Add(new ColorModel());
            }


            

            Task.Run(async () =>
            {
                await Task.Delay(3000);

                for (int i = 0; i < 5; i++)
                {
                    //这样才能通知到页面..
                    WHColors[i].Color = Brushes.ForestGreen;
                }
            });

        }

        private void  DoMenuCommand(object param)
        {
            // 菜单数据库维护
            // 每次创建的新？

            if (ContentView != null && ContentView.GetType().Name == param.ToString()) return;                                                                 
            Type type = Assembly.GetExecutingAssembly().GetType("我的地图.View.Pages." + param.ToString())!;
            this.ContentView = Activator.CreateInstance(type)!;
        }
    }



    public class ColorModel : ObservableValidator
    {
        private Brush _color = Brushes.ForestGreen;
       
        public Brush Color { get => _color; set { SetProperty(ref _color, value, true); } }

    }
}
