using BeautySaloon.Models;
using BeautySaloon.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BeautySaloon.Repositoryes;

namespace BeautySaloon.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _db;

		private readonly IAccoutRepository _accountRepository;

		

		public AccountController(IAccoutRepository repository, ApplicationDbContext db)
		{
			_accountRepository = repository;
			_db = db;

		}
		public async Task<ActionResult> Index()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			//var name = User.FindFirstValue("Name");
			//var phoneNumber = User.FindFirstValue("PhoneNumber");
			//var email = User.FindFirstValue(ClaimTypes.Email);
			var name = User.FindFirstValue(ClaimTypes.GivenName);
			var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);
			var email = User.FindFirstValue(ClaimTypes.Email);
			// Используйте значения по вашему усмотрению
			// Например, передайте их в представление

			ViewBag.UserId = userId;
			ViewBag.Name = name;
			ViewBag.PhoneNumber = phoneNumber;
			ViewBag.Email = email;
			var appointments = await _accountRepository.GetAppointmentsByUserIdAsync(userId);
			return View(appointments);
		}
		[HttpPost]
		public async Task<IActionResult> EditInfo(string Name, string Email, string Phone)
		{
			// Получение текущего пользователя
			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _db.User.FindAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			// Обновление полей пользователя
			user.Name = Name;
			user.Email = Email;
			user.PhoneNumber = Phone;

			// Сохранение изменений в базе данных
			_db.Update(user);
			await _db.SaveChangesAsync();
			await UpdateUserClaims(user);
			// Перенаправление на другую страницу
			return RedirectToAction("Index", "Account");
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(string NewPassword, string ConfirmPassword)
		{


			var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _db.User.FindAsync(userId);
			if (user == null)
			{
				return NotFound();
			}

			user.Password = NewPassword;


			await UpdateUserClaims(user);
			await _db.SaveChangesAsync();
			return RedirectToAction("Index", "Account");



		}
		private async Task UpdateUserClaims(User user)
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
		public async Task<IActionResult> DeleteAppointment(int id)
		{
			var appointmentToDelete = await _db.Appointment
				.FindAsync(id);

			if (appointmentToDelete != null)
			{
				_db.Appointment.Remove(appointmentToDelete);
				_db.SaveChanges();
				return RedirectToAction("Index", "Account");

			}
			else
			{
				return BadRequest();
			}
		}
	}
}
