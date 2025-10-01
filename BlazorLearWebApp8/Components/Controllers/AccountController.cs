using System.Security.Claims;
using BlazorLearWebApp8.Components.Pages;
using BlazorLearWebApp8.Components.Vo;
using BlazorLearWebApp8.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorLearWebApp8.Components.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AccoutController : ControllerBase
    {
        [HttpPost]
        public object Login([FromBody]LoginVo loginVo)
        {
            var user = UserEntity.Where(x => x.UserName == loginVo.UserName && x.Password == loginVo.Password).First();
            if (user == null)
            {
                return new { code = 50000, message = "用户名或者密码错误!" };
            }
            //设置COOKIE 名.
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));

            //如果保持就是5天，不就是30分钟吧..
            HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = loginVo.IsKeep ? DateTimeOffset.Now.AddDays(5) : DateTimeOffset.Now.AddMinutes(30)
            });
              

            return new { code = 20000, message = "登入成功" };
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }
    }
}
