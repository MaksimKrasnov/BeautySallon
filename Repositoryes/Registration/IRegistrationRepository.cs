using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes.Registration
{
	public interface IRegistrationRepository
	{
		Task<User?> GetUserByEmailAsync(string email);
		Task AddUserAsync(User user);
	}
}
