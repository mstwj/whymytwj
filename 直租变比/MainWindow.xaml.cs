using System.Text;
using System.Windows;
using 直租变比.Model;

namespace 直租变比
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Current_ProductNumber { get; set; } = null;

        Pagebb m_pagebb = new Pagebb();
        Pagezz m_pagezz = new Pagezz();

        //这里m_mainproinfo--主要的信号..
        //newproinfotable? m_mainproinfo = null;

        public MainWindow()
        {
            InitializeComponent();
        }


        private async void DoBtnMenu1_1Click(object sender, RoutedEventArgs e)
        {
            new AddProc().ShowDialog();
        }

        private async void DoBtnMenu1_2Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private async void DoBtnMenu4_1Click(object sender, RoutedEventArgs e)
        {
            new DevedWindow().ShowDialog();
        }

        private async void DoBtnMenu4_2Click(object sender, RoutedEventArgs e)
        {

        }


        private async void DoBtnMenu2_1Click(object sender, RoutedEventArgs e)
        {
            this.myImage.Visibility = Visibility.Hidden;
            //if (m_mainproinfo == null) { MessageBox.Show("必须先设置样品!"); return; }
            //m_pagebb.m_pagebbModel.m_mainproinfo = m_mainproinfo;
            //m_pagebb.m_pagebbModel.InitializeAsync();
            FrameNv.Navigate(m_pagebb);
        }

        private async void DoBtnMenu2_2Click(object sender, RoutedEventArgs e)
        {
            this.myImage.Visibility = Visibility.Hidden;
            FrameNv.Navigate(m_pagezz);
        }

    }
}