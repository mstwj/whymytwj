using IsWorkabing.Data;
using Microsoft.AspNetCore.Mvc;

namespace IsWorkabing.Controllers
{
    public class AlarmDatasController : Controller
    {
        private readonly AppDbContext _context;

        public AlarmDatasController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
