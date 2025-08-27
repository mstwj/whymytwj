using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;

namespace MyIPHoneNonChang.ViewModels.Dialog
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    
        public Command ReturnCommand { get; set; }

        public SettingsViewModel(IPopupService popupService)
        {
            ReturnCommand = new Command(() =>
            {
                popupService.ClosePopup();
            });
        }

    }
}
