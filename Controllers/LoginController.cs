using BeautySaloon.Models;
using BeautySaloon.Repositoryes.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeautySaloon.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILoginRepository _loginRepository;

		public LoginController(ILoginRepository loginRepository)
		{
			_loginRepository = loginRepository;
		}
		// GET: LoginController
		public ActionResult Index()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				User? user = await _loginRepository.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
				if (user != null)
				{
					await Authenticate(user);

					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			return View(model);
		}
		private async Task Authenticate(User user)
		{

			var claims = new List<Claim>
	{
		new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
		new Claim(ClaimTypes.GivenName, user.Name),
		new Claim(ClaimTypes.Email, user.Email),
		new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
	};

			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
