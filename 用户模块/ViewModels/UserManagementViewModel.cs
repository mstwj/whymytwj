using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using Unity;
using 公共类;
using 用户模块.Models;
using 车库客户端WPF接口类库.Interface;

namespace 用户模块.ViewModels
{
    public class UserManagementViewModel: ViewModelBase
    {
        public string PageTitle { get; set; } = "用户信息管理";

        //使用的时候如果没有 IOC 注入，你真不知道哪里有问题..
        IUserBLL _dataAccess;

        public ObservableCollection<UserModel> UserList { get; set; }
        IDialogService _dailogService;

        //鸡肋不会自己的去注入的，IOC只对页面来的。。。
        //这里有个大问题，就是调试的时候，如果你添加了IOC注入，就不知道有什么问题。断点不会断，也不提示你问题。
        //就是断不下来，你也不知道 到底哪里有问题..
        public UserManagementViewModel(IUserBLL dataAccess,
            IUnityContainer unityContainer, IRegionManager regionManager
            ,IDialogService dailogService)
            : base(unityContainer, regionManager)
        {
            _dataAccess = dataAccess;
            _dailogService = dailogService;
            UserList = new ObservableCollection<UserModel>();

            Refresh();
        }

        public override void Refresh()
        {
            //清空数据..
            UserList.Clear();

            //看登入是怎么写的..
            try
            {

                Task.Run(async () =>
                {
                    //我卡这里了，老师也卡这里了..
                    var dataStr = await _dataAccess.GetAll();
                    //这样直接拿就报错了..
                    var userEntity = JsonConvert.DeserializeObject<List<UserDetailedEntity>>(dataStr);

                    //这里没有空的判断..

                    foreach (var item in userEntity)
                    {
                        UserModel userModel = new UserModel
                        {
                            Index = 0,
                            UserId = item.id,
                            UserIcon = "pack://application:,,,/Assets;component/Images/avatar.png",
                            UserName = item.UserName,
                            Password = item.Password,
                            Age = item.Age,
                            RealName = item.RealName
                        };

                        var roles = await _dataAccess.GetRolesByUserId(userModel.UserId);
                        //这样直接拿就报错了..
                        var Rolesk = JsonConvert.DeserializeObject<List<RoleskEnitiy>>(roles);

                        Rolesk?.ForEach(r => userModel.Roles.Add(new RoleModel
                        {
                            RoleId = r.RoleId,
                            RoleName = r.RoleName,
                            State = 1
                        }));

                        userModel.EditCommand = new DelegateCommand<object>(EditItem);
                        userModel.DeleteCommand = new DelegateCommand<object>(DeleteCommand);
                        userModel.PwdCommand = new DelegateCommand<object>(PwdCommand);
                        userModel.RoleCommand = new DelegateCommand<object>(RoleCommand);

                        //为什么报错，因为不是主线程在ADD吗?
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UserList.Add(userModel);
                        });
                        



                    }
                });


                //这里要通过接口得到数据库里面的数据...
                //_dataAccess.GetAll();-- 使用主线程去拿数据吗？ 
                //var dataStr = _dataAccess.GetAll().GetAwaiter().GetResult();

                //UserDetailedEntity userEntity = JsonConvert.DeserializeObject<UserDetailedEntity>(dataStr);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                      
        }

        public override void AddItem()
        {

            base.AddItem();
        }

        private void EditItem(object obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("model", obj as UserModel);
            //这里是自己编辑自己..
            _dailogService.ShowDialog("ModeByUserDailog",param,
                result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        MessageBox.Show("数据保存OK");
                        this.Refresh();
                    }

                    if (result.Result == ButtonResult.Cancel)
                    {
                        MessageBox.Show("用户点击取消");
                    }

                    if (result.Result == ButtonResult.None)
                    {
                        MessageBox.Show("用户点击X");
                    }
                });
        }
        
        private void DeleteCommand(object obj)
        {
            //这里是自己编辑自己..

        }

        private void PwdCommand(object obj)
        {
            //这里最好要转呀转的...
            //这里是自己编辑自己..
            Task.Run(async () =>
            {
                //...
                System.Windows.MessageBox.Show("密码设置OK");
            });
        }

        private void RoleCommand(object obj)
        {
            //这里是自己编辑自己..

        }
    }
}
