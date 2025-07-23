using Microsoft.AspNetCore.Mvc;
using TailuopaiWeabapi.Base;

namespace TailuopaiWeabapi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public MyData GetAnswer(string id)
        {
            //http://localhost:5000/api/Home/GetAnswer            
            //http://localhost:5000/api/Home/GetAnswer/id=你好每个人..
            //http://twjgod.w1.luyouxia.net/api/GetAnswer/id=你好每个人..            
            string originalString = id;
            string newString = originalString.Substring(3);            

            string result =MyTools.GetTableAnswer(newString);
            if ("添加成功" == result) return new MyData() { data4 = "服务器繁忙,请以后在来..", presets1 = "请大家多多支持我:我的小店地址https://xxxx.xxx专门出售辟邪产品", presets2 = "未命中" };
            if ("添加失败" == result) return new MyData() { data4 = "服务器压力太大-可能已经崩溃...", presets1 = "请大家多多支持我:我的小店地址https://xxxx.xxx专门出售辟邪产品", presets2 = "未命中" };
            return new MyData() { data4 = result, presets1= "请大家多多支持我:我的小店地址https://xxxx.xxx专门出售辟邪产品", presets2 = "命中" }; 
        }
    }
}
