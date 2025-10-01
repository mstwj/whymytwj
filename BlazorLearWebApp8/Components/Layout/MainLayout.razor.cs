using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using BlazorLearWebApp8.Components.Pages;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace BlazorLearWebApp8.Components.Layout
{
    public partial class MainLayout
    {

        private ClaimsPrincipal? _user;


        private string? Theme { get; set; }

        private string? LayoutClassString => CssBuilder.Default("layout-demo")
            .AddClass(Theme)
            .Build();

        private IEnumerable<MenuItem>? Menus { get; set; }

        /// <summary>
        /// 获得/设置 是否固定页头
        /// </summary>
        public bool IsFixedHeader { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否固定页脚
        /// </summary>
        public bool IsFixedFooter { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否固定页脚
        /// </summary>
        public bool IsFixedTabHeader { get; set; } = false;

        /// <summary>
        /// 获得/设置 侧边栏是否外置
        /// </summary>
        public bool IsFullSide { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否显示页脚
        /// </summary>
        public bool ShowFooter { get; set; } = true;

        /// <summary>
        /// 获得/设置 是否开启多标签模式
        /// </summary>
        public bool UseTabSet { get; set; } = true;

        //当 ?. 左侧的对象为null 时，整个表达式的计算结果就是null。 
        //如果左侧对象非null，则继续执行右侧的成员访问
        /// <summary>
        /// OnInitializedAsync 方法
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;

            // 模拟异步获取菜单数据
            await Task.Delay(10);

            Menus = new List<MenuItem>
        {
            new() { Text = "首页", Icon = "fa-fw fa-solid fa-house", Url = "/" },
            new() { Text = "用户管理", Icon = "fa-fw fa-solid fa-desktop", Url = "/user" },
            new() { Text = "示例网页", Icon = "fa-fw fa-solid fa-laptop", Url = "layout-demo/text=Parameter1" }
        };
        }

        /// <summary>
        /// 更新组件方法
        /// </summary>
        public void Update() => StateHasChanged();
    }
}