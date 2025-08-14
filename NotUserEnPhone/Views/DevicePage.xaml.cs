using NotUserEnPhone.ViewModels;
using static Android.Graphics.ColorSpace;

namespace NotUserEnPhone.Views;

public partial class DevicePage : ContentPage
{
    //û��ʹ����Ϣ���ģ����ģ������ķ�ʽ ������
    DeviceViewModel model = new DeviceViewModel();
    public DevicePage()
	{
		InitializeComponent();

		this.BindingContext = model;
	}

	private void CarouselView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
	{
        // ��Ϊ����¼��ᴥ�����
        if (e.CenterItemIndex == e.LastVisibleItemIndex)
        {
            // ��������ʱ����ʾ�������
            // ���ﻹʼ������ʾ��Ϣ
            // ��Ϣ���ģ���ģ�����ݽ��������������Ѿ�׼�����������
            //MessagingCenter.Send();
            model.SwtichContent(e.LastVisibleItemIndex);
        }
    }
}