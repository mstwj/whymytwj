using IsWorkabing.Data;
using IsWorkabing.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace IsWorkabing.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context ,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            /*
            // 查询符合条件的报警数据
            var query = _context.test1
                .Where(a => a.Id == 3);

            Carbonemission test1ViewModel = new Test1ViewModel();

            if (query.Count() > 0) {
                test1 data = query.First();
                test1ViewModel.Id = data.Id;
                test1ViewModel.Avoltage = data.Avoltage;
                test1ViewModel.Bvoltage = data.Bvoltage;
                test1ViewModel.Cvoltage = data.Cvoltage;
                test1ViewModel.Aelectric = data.Aelectric;
                test1ViewModel.Belectric = data.Belectric;
                test1ViewModel.Celectric = data.Celectric;
                test1ViewModel.Aactivepower = data.Aactivepower;
                test1ViewModel.Bactivepower = data.Bactivepower;
                test1ViewModel.Cactivepower = data.Cactivepower;
            }
            
            */

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }

    
    // 报警数据视图模型(碳排放.. 1天 24小时，一共24个电..)
    public class Carbonemission
    {
        int Max;
        float[] floatArrayOld = new float[24]; //
        float[] floatArrayNew = new float[24]; //
    }

    //用电量 (1天 24小时，一共24个电..)
    public class PowerConsumption
    {
        int Max;
        float[] floatArrayOld = new float[24]; //
        float[] floatArrayNew = new float[24]; //
    }



    //节能收益(1天 24小时)
    public class EnergyConservation
    {
        int Max;
        float[] floatArray = new float[24]; //        
    }

    //智能分析..(1个月 -- 30天的数据)
    public class Analysis
    {
        int Max;
        float[] floatArrayOld = new float[30]; //
        float[] floatArrayNew = new float[30]; //
    }

    //当前功率..
    public class CurrentPower
    {
        float L1; //电流
        float L2; //电流
        float L3; //电流

        float V1; //电压
        float V2; //电压
        float V3; //电压

        float P1; //功率
        float P2; //功率
        float P3; //功率

    }

    //每日故障表..(这里就是一个循环滚动的自定义数据.)
    public class OnedayBug
    {
        //...(自己编写)
    }

    //当前客户收益.
    public class Customer
    {
        //...(自己编写)
        
    }



}
