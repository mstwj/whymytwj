using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace 学习WebApi的通讯方式.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{v}")]
        public IActionResult Get(string v)
        {
            return Ok("Hello"+v);
        }
    }
}
