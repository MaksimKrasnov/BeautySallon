using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Repositoryes
{

	public class SearchRepository : ISearchRepository
	{
		private readonly ApplicationDbContext _db;

		public SearchRepository(ApplicationDbContext context)
		{
			_db = context;
		}

		public async Task<IEnumerable<Service>> GetService()
		{
			return await _db.Service.Where(s => s.ServicesId == 1).ToListAsync();
		}
		public async Task<IEnumerable<Services>> GetServices()
		{
			return await _db.Services.ToListAsync();
		}
		public async Task<IEnumerable<Service>> Search(string searchString)
		{
			return await _db.Service
                .Where(ap => ap.Name.Contains(searchString))
                .ToListAsync();
		}
	}

}


