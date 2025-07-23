using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using 服务端访问数据类库;
using 服务端访问数据类库.Interfacdal;
using 服务端访问数据类库.Interface;
using 服务端访问数据类库.表;

namespace 汽车服务端.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IMenuService _menuService;
        IUserService _userService;
        public UserController(IMenuService menuService, IUserService userService)
        {
            _menuService = menuService;
            _userService = userService;
        }

        //http://localhost:6687/api/User/login
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromForm] string username, [FromForm]string password)
        {
            using (var context = new EFCoreContext())
            {
                SystemUserInfo firstEntity = context.SysUserInfo.FirstOrDefault(e => e.UserName == username && e.Password == password);
                if (firstEntity != null)
                {
                    //这里是通过接口来拿到数据库数据的..
                    List<MenuInfo> menus = _menuService.GetAllMenus();
                    firstEntity.Menus = menus;

                    //找到了..
                    return Ok(firstEntity);
                }
                else
                {
                    //没找到..
                    //MessageBox.Show("用户名或密码错误!");
                    return NoContent();
                }
            }

            return NoContent();
        }


        //
        [HttpGet]
        [Route("all")]
        public JsonResult GetUsers()
        {
            return Json(_userService.Query<UserDescInformat>(u=>true));
            
        }

        [HttpGet("roles/{userId}")]
        public JsonResult GetRolesByUserId(int userId)
        {
            return Json(_userService.GetRolesByUserId(userId));
        }

    }
}
