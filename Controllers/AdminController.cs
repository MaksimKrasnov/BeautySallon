using BeautySaloon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System.Globalization;


namespace BeautySaloon.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext db;

		public AdminController(ApplicationDbContext context)
		{
			db = context;
		}

		public async Task<ActionResult> Index(DateTime selectedDate, int serviceId)
		{

			List<Master> masters = new List<Master>();

			if (serviceId == 0)
			{
				masters = await db.Master.ToListAsync();

			}

			else
			{
				masters = await db.MasterService
								.Where(ms => ms.ServiceId == serviceId)
								.Select(ms => ms.Master)
								.ToListAsync();
			}
			List<Service> services = db.Service.ToList();
			if (selectedDate == DateTime.ParseExact("01.01.0001 0:00:00", "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture))
			{
				selectedDate = DateTime.Now;
			}


			// Генерируем список временных интервалов с 8:00 до 20:00
			Dictionary<Master, Dictionary<DateTime, string>> dict = new Dictionary<Master, Dictionary<DateTime, string>>();
			foreach (var master in masters)
			{
				var appointmentsOnSelectedDate = await db.Appointment
							 .Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService.MasterId == master.Id)
							 .ToListAsync();
				var currentTime = selectedDate.Date.AddHours(8); // Начальное время записи

				Dictionary<DateTime, string> d = new Dictionary<DateTime, string>();
				while (currentTime.Hour < 20) // Пока не достигнем 20:00
				{
					// Проверяем, занято ли текущее время для данного мастера
					if (appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour))
					{
						d.Add(currentTime, "Занято");

					}
					else
					{
						d.Add(currentTime, "Свободно");
					}

					// Переходим к следующему часу
					currentTime = currentTime.AddHours(1);
				}
				dict.Add(master, d);
			}

			// Создаем модель представления
			var viewModel = new AdminViewModel
			{
				Services = services,
				Masters = masters,
				Appointments = dict,
				ServiceId = serviceId,
				Date = selectedDate
			};

			return View(viewModel);
		}

		public async Task<ActionResult> DeleteAppointment(DateTime selectedDate, int masterId)
		{
			var appointmentToDelete = await db.Appointment
				.FirstOrDefaultAsync(a => a.DateTime == selectedDate && a.MasterService.MasterId == masterId);

			if (appointmentToDelete != null)
			{
				db.Appointment.Remove(appointmentToDelete);
				db.SaveChanges();
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
