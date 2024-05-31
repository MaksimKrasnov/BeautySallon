using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeautySaloon.Repositoryes
{
	public class AccountRepository: IAccoutRepository
	{
		private readonly ApplicationDbContext _context;

		public AccountRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
		{
			var appointments = await _context.Appointment
	.Where(a => a.UserId == userId)
	.OrderByDescending(a => a.DateTime)
	.ToListAsync();

			// Получаем Id всех MasterService для всех записей Appointment
			var masterServiceIds = appointments.Select(a => a.MasterServiceId).ToList();

			// Загружаем все MasterService по Id
			var masterServices = await _context.MasterService
				.Include(ms => ms.Master)
				.Include(ms => ms.Service)
				.Where(ms => masterServiceIds.Contains(ms.Id))
				.ToListAsync();

			// Заполняем связанные сущности для каждого Appointment
			foreach (var appointment in appointments)
			{
				appointment.MasterService = masterServices.FirstOrDefault(ms => ms.Id == appointment.MasterServiceId);
			}
			return  appointments;
				
		}
	}
}
