using BeautySaloon.Email;
using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Repositoryes
{
	public class HomeRepository: IHomeRepository
	{
		private readonly ApplicationDbContext _db;

		public HomeRepository(ApplicationDbContext context)
		{
			_db = context;
		}

		public async Task<IEnumerable<Service>> GetServiceById(int id)
		{
			return await _db.Service.Where(s => s.ServicesId == id).ToListAsync();
		}

		public async Task<IEnumerable<Services>> GetAllServices()
		{
			return await _db.Services.ToListAsync();
		}


		public async Task SendMessage(string name, string phone)
		{
			EmailService email = new EmailService();
			await email.SendMail("krasnovm020@gmail.com", "Перезвонить клиенту", $"Имя клиента: {name} телефон: {phone} ");
		}
	}
}
