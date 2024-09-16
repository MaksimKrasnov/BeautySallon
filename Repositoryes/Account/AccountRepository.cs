using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeautySaloon.Repositoryes.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository() { }
        public AccountRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId)
        {
            var appointments = await _db.Appointment
    .Where(a => a.UserId == userId)
    .OrderByDescending(a => a.DateTime)
    .ToListAsync();

            // Получаем Id всех MasterService для всех записей Appointment
            var masterServiceIds = appointments.Select(a => a.MasterServiceId).ToList();

            // Загружаем все MasterService по Id
            var masterServices = await _db.MasterService
                .Include(ms => ms.Master)
                .Include(ms => ms.Service)
                .Where(ms => masterServiceIds.Contains(ms.Id))
                .ToListAsync();

            // Заполняем связанные сущности для каждого Appointment
            foreach (var appointment in appointments)
            {
                appointment.MasterService = masterServices.FirstOrDefault(ms => ms.Id == appointment.MasterServiceId);
            }
            return appointments;

        }
        public async Task<User> FindUserByIdAsync(int userId)
        {
            return await _db.User.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _db.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var appointmentToDelete = await _db.Appointment.FindAsync(appointmentId);
            if (appointmentToDelete != null)
            {
                _db.Appointment.Remove(appointmentToDelete);
                await _db.SaveChangesAsync();
            }
        }
		public async Task<List<Appointment>> LoadMoreAppointments(int skip, int userId,int take = 10)
		{
            var appointments = await _db.Appointment
              .Where(a => a.UserId == userId)
              .OrderByDescending(a => a.DateTime)
              .Skip(skip)
              .Take(take)
              .ToListAsync();
            var masterServiceIds = appointments.Select(a => a.MasterServiceId).ToList();

			// Загружаем все MasterService по Id
			var masterServices = await _db.MasterService
				.Include(ms => ms.Master)
				.Include(ms => ms.Service)
				.Where(ms => masterServiceIds.Contains(ms.Id))
				.ToListAsync();

			// Заполняем связанные сущности для каждого Appointment
			foreach (var appointment in appointments)
			{
				appointment.MasterService = masterServices.FirstOrDefault(ms => ms.Id == appointment.MasterServiceId);
			}
			return appointments;


		}
	}
}
