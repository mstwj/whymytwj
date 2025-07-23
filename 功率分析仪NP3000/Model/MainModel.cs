using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace 功率分析仪NP3000.Model
{
    class MainModel : ObservableObject
    {        
        public MainModel()
        {
            
                        
        }

        private void DobtnCommandPortSet(object button)
        {
            DeveclWindow dialog = new DeveclWindow();
            dialog.ShowDialog();
            return;
        }

    }

}