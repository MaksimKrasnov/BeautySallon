using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeautySaloon.Models;
using BeautySaloon.Email;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeautySaloon.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly ApplicationDbContext db;

		public AppointmentController(ApplicationDbContext context)
		{
			db = context;
		}

		// GET: Appointment
		public async Task<IActionResult> Index(int id)
		{
			var masters = await db.MasterService
				   .Include(ms => ms.Master)
				   .Where(ms => ms.ServiceId == id)
				   .Select(ms => ms.Master)
				   .ToListAsync();

			// Передаем список мастеров в представление
			ViewBag.Masters = new SelectList(masters, "Id", "Name");
			var service = await db.Service.FirstOrDefaultAsync(s => s.Id == id);

			return View(service);

		}

		public async Task<IActionResult> GetTimeSlot(DateTime selectedDate, int masterId)
		{

			//  Получаем все записи на выбранный день
			var appointmentsOnSelectedDate = await db.Appointment
		  .Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService.MasterId == masterId)
		  .ToListAsync();

			// Генерируем доступные временные интервалы с 08:00 до 20:00 для выбранного мастера
			var availableTimeSlots = new List<DateTime>();
			var currentTime = selectedDate.Date.AddHours(8); // Начальное время записи

			while (currentTime.Hour < 20) // Пока не достигнем 20:00
			{
				// Проверяем, занято ли текущее время для данного мастера
				if (!appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour))
				{
					availableTimeSlots.Add(currentTime);
				}

				// Переходим к следующему часу
				currentTime = currentTime.AddHours(1);
			}

			return PartialView("TimeSlotPartView", availableTimeSlots);
		}

		[HttpPost]
		public async Task<IActionResult> Create(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime)
		{
			if (selectedDateTime == DateTime.MinValue)
			{
				return BadRequest("Выберите корректную дату и время.");
			}
			if (nameInput != null && phoneInput != null)
			{
				try
				{
					var masterServiceId = await db.MasterService
			.Where(ms => ms.ServiceId == serviceId && ms.MasterId == masterId)
			.Select(ms => ms.Id)
			.FirstOrDefaultAsync();
					Appointment app = new Appointment { DateTime = selectedDateTime, MasterServiceId = masterServiceId, ClientName = nameInput, Phone = phoneInput };
					db.Appointment.Add(app);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
				EmailService email = new EmailService();
				email.SendMail("krasnovm020@gmail.com", "Запись ", $"Имя клиента: {nameInput} телефон: {phoneInput}  дата и время:{selectedDateTime}");

				return Ok();
			}
			return BadRequest("Не заполнены поля");

		}

	}
}
