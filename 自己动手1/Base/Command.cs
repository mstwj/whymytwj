using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 自己动手1.Base
{
    public class Command<T> : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) 
        {
            if (DoCanExecute == null)
            {
                return true;
            }
            return DoCanExecute(parameter);
        }


        public void Execute(object? parameter)
        {
            DoExecuteNoneParam?.Invoke();

            dynamic p = parameter;
            DoExecuteWithParam?.Invoke(p);
        }

        public void RasieCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this,new EventArgs());
        }


        public Action DoExecuteNoneParam { get; set; }
        public Action<T> DoExecuteWithParam { get; set; }

        public Func<object,bool> DoCanExecute { get; set; }

        public Command(Action doExecute)
        {
            DoExecuteNoneParam = doExecute;
        }
        public Command(Action<T> doExecute)
        {
            DoExecuteWithParam = doExecute;
        }
    }
}
