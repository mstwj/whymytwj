using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLC自己动手2.Base;
using PLC自己动手2.Models;

namespace PLC自己动手2.ViewModels
{
    public class MainViewModel
    {
        public MainModel MainModel { get; set; } = new MainModel();

        private CommandBase _closeCommand;

        public CommandBase CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new CommandBase();
                    _closeCommand.DoExecute = new Action<object>(obj =>
                    {
                        (obj as System.Windows.Window).DialogResult = false;
                    });
                }
                return _closeCommand;
            }
        }

        private CommandBase _menuItemCommand;

        public CommandBase MenuItemCommand
        {
            get
            {
                if (_menuItemCommand == null)
                {
                    _menuItemCommand = new CommandBase();
                    _menuItemCommand.DoExecute = new Action<object>(obj =>
                    {
                        NavPage(obj.ToString());
                    });
                }
                return _menuItemCommand;
            }
        }

        private void NavPage(string name)
        {
            Type type = Type.GetType(name);
            //这里他去创建了一个UI对象...
            this.MainModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);
        }

        public MainViewModel()
        {
            this.NavPage("PLC自己动手2.Views.MonitorView");

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    this.MainModel.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            });
        }

    }
}
