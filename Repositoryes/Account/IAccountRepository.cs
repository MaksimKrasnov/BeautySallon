using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes.Account
{
    public interface IAccountRepository
    {
        public Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
        Task<User> FindUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task DeleteAppointmentAsync(int appointmentId);

    }
}
