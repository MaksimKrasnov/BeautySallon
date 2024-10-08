﻿
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BeautySaloon.Models;
using System.Security.Claims;
using BeautySaloon.Repositoryes.AppointmentRep;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Controllers
{
    public class AppointmentController : Controller
	{
		private readonly ApplicationDbContext _db;


		private readonly IAppointmentRepository _appointmentRepository;

		public AppointmentController(IAppointmentRepository repository, ApplicationDbContext db)
		{
			_appointmentRepository = repository;
			_db = db;

		}

		// GET: Appointment
		public async Task<IActionResult> Index(int id)
		{
			var masters = await _appointmentRepository.GetMastersByServiceId(id);
			ViewBag.Masters = new SelectList(masters, "Id", "Name");
			var service = await _appointmentRepository.GetServiceById(id);

			var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
			var name = User.FindFirstValue(ClaimTypes.GivenName);
			var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);

			ViewBag.UserId = userId;
			ViewBag.Name = name;
			ViewBag.PhoneNumber = phoneNumber;
			return View(service);
		}

		public async Task<IActionResult> GetTimeSlot(DateTime selectedDate, int masterId) //Получение временных интервалов работы каждого мастера
		{
            var isMasterWorking = await _appointmentRepository.IsMasterWorkingOnDate(selectedDate, masterId);

            if (!isMasterWorking)
            {
                // Если мастер не работает в выбранную дату, возвращаем пустой список 
                return PartialView("TimeSlotPartView", new List<DateTime>());
                
            }

            // Если мастер работает, получаем доступные временные слоты
            var availableTimeSlots = await _appointmentRepository.GetAvailableTimeSlots(selectedDate, masterId);
            return PartialView("TimeSlotPartView", availableTimeSlots);
        }
    

		[HttpPost]
		
		public async Task<IActionResult> Create(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime)//Добавление записи на выбранную дату и время
		{
			var normalDate = ConvertTo24HourFormat(selectedDateTime.ToString());

			if (normalDate == DateTime.MinValue)
			{
				return BadRequest("Выберите корректную дату и время.");
			}

			int userId = 1;
			if (User?.Identity?.IsAuthenticated == true)
			{
				userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			}

			if (!string.IsNullOrEmpty(nameInput) && !string.IsNullOrEmpty(phoneInput))
			{
				using (var transaction = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
				{
					try
					{
						bool isCreated = await _appointmentRepository.CreateAppointment(serviceId, masterId, nameInput, phoneInput, userId, normalDate);
						if (isCreated)
						{
							await transaction.CommitAsync();
							return Ok();
						}
						else
						{
							await transaction.RollbackAsync();
							return BadRequest("Не удалось создать запись.");
						}
					}
					catch (Exception ex)
					{
						await transaction.RollbackAsync();
						return BadRequest(ex.Message);
					}
				}
			}

			return BadRequest("Не заполнены поля");
		}
		public static DateTime ConvertTo24HourFormat(string dateInput)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{
				return dateTime;
			}
			else
			{
				return Convert.ToDateTime(dateInput);
			}
		}
	}
}
