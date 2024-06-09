using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes.Home
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Service>> GetServiceById(int id);
        Task<IEnumerable<Services>> GetAllServices();
        Task SendMessage(string name, string phone);
    }
}
