using NotUserEnPhone.ViewModels;
using static Android.Graphics.ColorSpace;

namespace NotUserEnPhone.Views;

public partial class DevicePage : ContentPage
{
    //没有使用消息中心，订阅，发布的方式 。。。
    DeviceViewModel model = new DeviceViewModel();
    public DevicePage()
	{
		InitializeComponent();

		this.BindingContext = model;
	}

	private void CarouselView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
	{
        // 因为这个事件会触发多次
        if (e.CenterItemIndex == e.LastVisibleItemIndex)
        {
            // 满足条件时，表示滚动完成
            // 这里还始调整显示信息
            // 消息中心（跨模块数据交互），这个框架已经准备好这个对象
            //MessagingCenter.Send();
            model.SwtichContent(e.LastVisibleItemIndex);
        }
    }
}