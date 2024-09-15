using BeautySaloon.Models;
using BeautySaloon.Repositoryes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeautySaloon.Repositoryes.Account;

namespace BeautySaloon.Controllers
{
    public class AccountController : Controller
	{

		private readonly IAccountRepository _accountRepository;

		

		public AccountController(IAccountRepository repository)
		{
			_accountRepository = repository;

		}
		public async Task<ActionResult> Index() //Основная страница, получаем данные о пользователе и его записях
		{
			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

		
			var name = User.FindFirstValue(ClaimTypes.GivenName);
			var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
			var email = User.FindFirstValue(ClaimTypes.Email);
			ViewBag.UserId = userId;
			ViewBag.Name = name;
			ViewBag.PhoneNumber = phoneNumber;
			ViewBag.Email = email;
			var appointments = await _accountRepository.GetAppointmentsByUserIdAsync(userId);
			return View(appointments);
		}
		[HttpPost]
		public async Task<IActionResult> EditInfo(string Name, string Email, string Phone) //Обновление информации о пользователе
		{
			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _accountRepository.FindUserByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			user.Name = Name;
			user.Email = Email;
			user.PhoneNumber = Phone.Replace(" ", "");

			await _accountRepository.UpdateUserAsync(user);
			await UpdateUserClaims(user);
			return RedirectToAction("Index", "Account");
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(string NewPassword, string ConfirmPassword) //Обновление пароля
		{


			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _accountRepository.FindUserByIdAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			user.Password = NewPassword;

			await _accountRepository.UpdateUserAsync(user);
			await UpdateUserClaims(user);
			return RedirectToAction("Index", "Account");



		}
		private async Task UpdateUserClaims(User user) //Обновлене инофрмации о пользователе в куках
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
		public async Task<IActionResult> DeleteAppointment(int id) //Отмена записи пользователем через ЛК
		{
			await _accountRepository.DeleteAppointmentAsync(id);
			return RedirectToAction("Index", "Account");
		}
		public async Task<IActionResult> LoadMoreAppointments(int skip, int take = 10) //Подгружаем по 10 записей  
		{
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var appointments = await _accountRepository.LoadMoreAppointments(skip, userId,take );

			return PartialView("AppointmentRows", appointments);
		}
	}
}
