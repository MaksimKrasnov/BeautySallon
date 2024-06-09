using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes.Login
{
	public interface ILoginRepository
	{
		Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
	}
}
