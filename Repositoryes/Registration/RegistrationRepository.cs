using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Repositoryes.Registration
{
	public class RegistrationRepository : IRegistrationRepository
	{
		private readonly ApplicationDbContext _db;

		public RegistrationRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			return await _db.User.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task AddUserAsync(User user)
		{
			_db.User.Add(user);
			await _db.SaveChangesAsync();
		}
	}
}
