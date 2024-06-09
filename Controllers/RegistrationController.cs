using BeautySaloon.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using BeautySaloon.Repositoryes.Registration;

namespace BeautySaloon.Controllers
{
    public class RegistrationController : Controller
    {
		private readonly IRegistrationRepository _registrationRepository;

		public RegistrationController(IRegistrationRepository registrationRepository)
		{
			_registrationRepository = registrationRepository;
		}
		public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterationModel model)
        {
            if (ModelState.IsValid)
            {
				User? user = await _registrationRepository.GetUserByEmailAsync(model.Email);
				if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Email = model.Email, Password = model.Password, PhoneNumber = model.PhoneNumber, Name = model.Name };



					await _registrationRepository.AddUserAsync(user);


					await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
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
    }
}
