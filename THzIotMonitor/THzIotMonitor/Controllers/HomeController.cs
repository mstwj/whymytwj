using Microsoft.AspNetCore.Mvc;
namespace THzIotPlatform.Controllers
{
    public class HomeController: Controller
    {
        // 默认动作方法（对应路由中的 action=Index）
        public IActionResult Index()
        {
            // 返回首页视图
            return View();
        }
    }
}
