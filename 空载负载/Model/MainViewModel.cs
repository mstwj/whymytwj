using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Modbus.Device;
using System.Windows;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using 空载负载.Base;
using System.Windows.Controls;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Modbus.Extensions.Enron;

using System.Windows.Markup;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using 空载负载.View;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection;
using 空载负载.Table;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace 空载负载.Model
{
    public class MainViewModel : ObservableValidator
    {
        public Table_ProductInfo m_sampleinformation { get; set; } = new Table_ProductInfo();
        public Table_NoloadStandardInfo m_sampleinNoload { get; set; } = new Table_NoloadStandardInfo();

        public Table_LoadStandardInfo m_sampleinLoad { get; set; } = new Table_LoadStandardInfo();

        public Table_InductionStandardInfo m_sampleinInduction { get; set; } = new Table_InductionStandardInfo();

        public Table_UtilityStandardInfo m_sampleinUtitiy { get; set; } = new Table_UtilityStandardInfo();

        private object _contentView;
        public object ContentView { get => _contentView; set { SetProperty(ref _contentView, value); } }

        private bool DoClosePre(string Name)
        {
            if (Name == "NoloadPage")
            {
                if (((NoloadPage)(ContentView)).model.Close() == false) return false;
                ContentView = null;
            }

            if (Name == "LoadPage")
            {
                if (((LoadPage)(ContentView)).model.Close() == false) return false;                
                ContentView = null;
            }

            if (Name == "InductionPage")
            {
                if (((InductionPage)(ContentView)).model.Close() == false) return false;                
                ContentView = null;
            }

            if (Name == "UtilityPage")
            {
                if (((UtilityPage)(ContentView)).model.Close() == false) return false;
                ContentView = null;
            }
            return true;
        }

        public bool DoInit(string Name)
        {
            if (Name == "NoloadPage")
            {
                try
                {
                    using (var context = new MyDbContext())
                    {
                        Table_NoloadStandardInfo firstEntity = context.NoloadStandardInfo.FirstOrDefault(e => e.ProductType == m_sampleinformation.ProductType && e.ProductTuhao == m_sampleinformation.ProductTuhao);
                        if (firstEntity == null)
                        {
                            MessageBox.Show("配置产品,在空载标准库里,没有找到这个标准!");
                            return false;
                        }
                        m_sampleinNoload.ProductType = firstEntity.ProductType;
                        m_sampleinNoload.ProductTuhao = firstEntity.ProductTuhao;
                        m_sampleinNoload.ProductStandard = firstEntity.ProductStandard;
                        m_sampleinNoload.ProductStandardUpperimit = firstEntity.ProductStandardUpperimit;
                        m_sampleinNoload.ProductStandardDownimit = firstEntity.ProductStandardDownimit;
                        m_sampleinNoload.ProductCurrentStandard = firstEntity.ProductCurrentStandard;
                        m_sampleinNoload.ProductCurrentStandardUpperrimit = firstEntity.ProductCurrentStandardUpperrimit;
                        m_sampleinNoload.ProductCurrentStandardDownimit = firstEntity.ProductCurrentStandardDownimit;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

                ((NoloadPage)(ContentView)).model.Initiate(m_sampleinformation, m_sampleinNoload);
            }

            if (Name == "LoadPage")
            {
                try
                {
                    using (var context = new MyDbContext())
                    {
                        Table_LoadStandardInfo firstEntity = context.LoadStandardInfo.FirstOrDefault(e => e.ProductType == m_sampleinformation.ProductType && e.ProductTuhao == m_sampleinformation.ProductTuhao);
                        if (firstEntity == null)
                        {
                            MessageBox.Show("配置产品,在负载标准库里,没有找到这个标准!");
                            return false;
                        }
                        m_sampleinLoad.ProductType = firstEntity.ProductType;
                        m_sampleinLoad.ProductTuhao = firstEntity.ProductTuhao;
                        m_sampleinLoad.Loadloss = firstEntity.Loadloss;
                        m_sampleinLoad.LoadlossUpperimit = firstEntity.LoadlossUpperimit;
                        m_sampleinLoad.LoadlossDownimit = firstEntity.LoadlossDownimit;
                        m_sampleinLoad.LoadMainReactance = firstEntity.LoadMainReactance;
                        m_sampleinLoad.LoadMainReactanceUpperrimit = firstEntity.LoadMainReactanceUpperrimit;
                        m_sampleinLoad.LoadMainReactanceDownimit = firstEntity.LoadMainReactanceDownimit;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

                ((LoadPage)ContentView).model.Initiate(m_sampleinformation, m_sampleinLoad);
            }

            if (Name == "InductionPage")
            {

               // ((InductionPage)(ContentView)).model.Initiate();
            }

            if (Name == "UtilityPage")
            {
              //  ((UtilityPage)(ContentView)).model.Initiate();
            }
            return true;
        }


        public bool DoBtnCommandStart(string param)
        {            
            if (ContentView != null && ContentView.GetType().Name == param) return false;
            if (ContentView != null)
            {
                if (DoClosePre(ContentView.GetType().Name) == false) return false;
            }
            Type type = Assembly.GetExecutingAssembly().GetType("空载负载.View." + param)!;
            this.ContentView = Activator.CreateInstance(type)!;
            return true;
        }


    }
}
