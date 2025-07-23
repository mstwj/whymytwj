using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using net5_10_14.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace net5_10_14.ViewModel
{
    public class MainViewModel
    {
        public ICommand btnCommand11 { get; set; } //
        public ICommand btnCommand12 { get; set; } //
        public ICommand btnCommand21 { get; set; } //
        public ICommand btnCommand22 { get; set; } //
        public ICommand btnCommand31 { get; set; } //
        public ICommand btnCommand32 { get; set; } //
        public ICommand btnCommand33 { get; set; } //
        public ICommand btnCommand34 { get; set; } //
        

        public ICommand btnCommand41 { get; set; } //
        public ICommand btnCommand42 { get; set; } //

        public MainViewModel()
        {
            btnCommand11 = new RelayCommand<object>(DobtnCommand11);
            btnCommand12 = new RelayCommand<object>(DobtnCommand12);
            btnCommand21 = new RelayCommand<object>(DobtnCommand21);
            btnCommand22 = new RelayCommand<object>(DobtnCommand22);
            btnCommand31 = new RelayCommand<object>(DobtnCommand31);
            btnCommand32 = new RelayCommand<object>(DobtnCommand32);
            btnCommand33 = new RelayCommand<object>(DobtnCommand33);
            btnCommand34 = new RelayCommand<object>(DobtnCommand34);
            btnCommand41 = new RelayCommand<object>(DobtnCommand41);
            btnCommand42 = new RelayCommand<object>(DobtnCommand42);

        }


        private void DobtnCommand11(object button) 
        {
            ActionStack.Execute("ShowDialogdialogYanPing", button);
            return; 
        }
        private void DobtnCommand12(object button) 
        {
            return; 
        }
        private void DobtnCommand21(object button)
        {
            return; 
        }
        private void DobtnCommand22(object button) { return; }
        private void DobtnCommand31(object button) 
        { 
            ActionStack.Execute("ShowDialogdialogBianbiReport2", button); 
            return;  
        }
        private void DobtnCommand32(object button) 
        {
            ActionStack.Execute("ShowDialogdialogZhizuReport2", button);
            return; 
        }

        private void DobtnCommand33(object button)
        {
            ActionStack.Execute("ShowDialogdialogBianbiBiaoReport", button); 
            return;
        }

        private void DobtnCommand34(object button)
        {
            ActionStack.Execute("ShowDialogdialogZhizuBiaoReport", button);            
            return;
        }

        private void DobtnCommand41(object button) 
        {
            ActionStack.Execute("ShowDialogdialogDevelWindows", button);
            return; 
        }
        private void DobtnCommand42(object button) 
        {
            ActionStack.Execute("ShowDialogdialogYanPingShowWindows", button);
            return;
        }

    }
}

