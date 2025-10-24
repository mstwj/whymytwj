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
            // 查询符合条件的报警数据
            var query = _context.test1
                .Where(a => a.Id == 3);
            Test1ViewModel test1ViewModel = new Test1ViewModel();

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


            return View(test1ViewModel);
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

    // 报警数据视图模型
    public class Test1ViewModel
    {
        public int Id { get; set; }  // 编号，自增

        public double Avoltage { get; set; }

        public double Bvoltage { get; set; }

        public double Cvoltage { get; set; }

        public double Aelectric { get; set; }

        public double Belectric { get; set; }

        public double Celectric { get; set; }

        public double Aactivepower { get; set; }

        public double Bactivepower { get; set; }
        public double Cactivepower { get; set; }
    }
}
