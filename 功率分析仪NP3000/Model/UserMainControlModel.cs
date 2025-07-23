using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace 功率分析仪NP3000.Model
{
    public class UserMainControlModel : ObservableObject
    {
        private float _currentnumber1_1 = 1.10f;
        public float CurrentNumber1_1 { get => _currentnumber1_1; set { SetProperty(ref _currentnumber1_1, value); } }

        private float _currentnumber1_2 = 1.20f;
        public float CurrentNumber1_2 { get => _currentnumber1_2; set { SetProperty(ref _currentnumber1_2, value); } }

        private float _currentnumber1_3 = 1.30f;
        public float CurrentNumber1_3 { get => _currentnumber1_3; set { SetProperty(ref _currentnumber1_3, value); } }

        private float _currentnumber1_4 = 1.40f;
        public float CurrentNumber1_4 { get => _currentnumber1_4; set { SetProperty(ref _currentnumber1_4, value); } }

        private float _currentnumber2_1 = 2.10f;
        public float CurrentNumber2_1 { get => _currentnumber2_1; set { SetProperty(ref _currentnumber2_1, value); } }

        private float _currentnumber2_2 = 2.20f;
        public float CurrentNumber2_2 { get => _currentnumber2_2; set { SetProperty(ref _currentnumber2_2, value); } }

        private float _currentnumber2_3 = 2.30f;
        public float CurrentNumber2_3 { get => _currentnumber2_3; set { SetProperty(ref _currentnumber2_3, value); } }

        private float _currentnumber2_4 = 2.40f;
        public float CurrentNumber2_4 { get => _currentnumber2_4; set { SetProperty(ref _currentnumber2_4, value); } }

        private float _currentnumber3_1 = 3.10f;
        public float CurrentNumber3_1 { get => _currentnumber3_1; set { SetProperty(ref _currentnumber3_1, value); } }

        private float _currentnumber3_2 = 3.20f;
        public float CurrentNumber3_2 { get => _currentnumber3_2; set { SetProperty(ref _currentnumber3_2, value); } }

        private float _currentnumber3_3 = 3.30f;
        public float CurrentNumber3_3 { get => _currentnumber3_3; set { SetProperty(ref _currentnumber3_3, value); } }

        private float _currentnumber3_4 = 3.40f;
        public float CurrentNumber3_4 { get => _currentnumber3_4; set { SetProperty(ref _currentnumber3_4, value); } }

        private float _currentnumber4_1 = 4.10f;
        public float CurrentNumber4_1 { get => _currentnumber4_1; set { SetProperty(ref _currentnumber4_1, value); } }

        private float _currentnumber4_2 = 4.20f;
        public float CurrentNumber4_2 { get => _currentnumber4_2; set { SetProperty(ref _currentnumber4_2, value); } }

        private float _currentnumber4_3 = 4.30f;
        public float CurrentNumber4_3 { get => _currentnumber4_3; set { SetProperty(ref _currentnumber4_3, value); } }

        private float _currentnumber4_4 = 4.40f;
        public float CurrentNumber4_4 { get => _currentnumber4_4; set { SetProperty(ref _currentnumber4_4, value); } }

        private float _averagevoltage = 5.10f;
        public float Averagevoltage { get => _averagevoltage; set { SetProperty(ref _averagevoltage, value); } }

        private float _frequency = 6.10f;
        public float Frequency { get => _frequency; set { SetProperty(ref _frequency, value); } }

        private float _correct = 6.10f;
        public float Correct { get => _correct; set { SetProperty(ref _correct, value); } }

        private float _loadcurrent = 7.10f;

        public float Loadcurrent { get => _loadcurrent; set { SetProperty(ref _loadcurrent, value); } }

        private float _testingvoltage = 8.9f;
        public float Testingvoltage { get => _testingvoltage; set { SetProperty(ref _testingvoltage, value); } }

        private float _protectionvoltage = 8.2f;
        public float Protectionvoltage { get => _protectionvoltage; set { SetProperty(ref _protectionvoltage, value); } }

        private float _protectioncurrent = 8.5f;
        public float Protectioncurrent { get => _protectioncurrent; set { SetProperty(ref _protectioncurrent, value); } }        

        public string comboBox1str { get; set; }
        public string comboBox2str { get; set; }
        public string comboBox3str { get; set; }


    }
}
