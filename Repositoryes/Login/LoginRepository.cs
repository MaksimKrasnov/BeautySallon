using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Repositoryes.Login
{
	public class LoginRepository : ILoginRepository
	{
		private readonly ApplicationDbContext _db;

		public LoginRepository(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
		{
			return await _db.User
				.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
		}
	}
}