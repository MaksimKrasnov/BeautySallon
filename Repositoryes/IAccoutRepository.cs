using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes
{
	public interface IAccoutRepository
	{
		public  Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);

	}
}
