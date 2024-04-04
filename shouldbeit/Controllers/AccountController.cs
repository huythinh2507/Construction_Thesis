using Castle.Core.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Thesis_web.Controllers
{
    public class AccountController : Controller
    {
        

    
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
		public IActionResult Logout()
		{
			// Clear the session
			HttpContext.Session.Clear();

			// Clear the cookies
			if (HttpContext.Request.Cookies["user_id"] != null)
			{
				HttpContext.Response.Cookies.Delete("user_id");
			}
			if (HttpContext.Request.Cookies["username"] != null)
			{
				HttpContext.Response.Cookies.Delete("username");
			}

			// Redirect to the login page (or any other page you want)
			return RedirectToAction("Index", "Home");
		}






	}

}
