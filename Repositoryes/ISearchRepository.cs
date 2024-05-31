using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes
{
	public interface ISearchRepository
	{
		Task<IEnumerable<Service>> GetService();
		Task<IEnumerable<Services>> GetServices();
		Task<IEnumerable<Service>> Search(string searchString);

	}
}
