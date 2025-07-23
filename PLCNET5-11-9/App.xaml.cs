using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PLCNET5_11_9
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "StikyNotesAPPMy", out ret);

            if (!ret)
            {
                MessageBox.Show("程序已经运行了");
                Environment.Exit(0);
            }

            base.OnStartup(e);

        }
    }

    public class LanguageManager
    {
        public static readonly DependencyProperty LanguageProperty = DependencyProperty.RegisterAttached(
            "Language", typeof(string), typeof(LanguageManager), new PropertyMetadata(string.Empty, OnLanguageChanged));

        public static string GetLanguage(DependencyObject obj)
        {
            return (string)obj.GetValue(LanguageProperty);
        }

        public static void SetLanguage(DependencyObject obj, string value)
        {
            obj.SetValue(LanguageProperty, value);
        }

        private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string language = e.NewValue as string;
            ResourceDictionary resourceDictionary = new ResourceDictionary();

            // 根据语言加载不同的资源字典
            if (language == "en")
            {
                resourceDictionary.Source = new Uri("pack://application:,,,/Strings.en.xaml", UriKind.RelativeOrAbsolute);
            }
            else
            {
                resourceDictionary.Source = new Uri("pack://application:,,,/Strings.zh.xaml", UriKind.RelativeOrAbsolute);
            }

            // 应用资源字典到资源
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}
