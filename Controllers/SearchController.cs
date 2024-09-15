using Microsoft.AspNetCore.Mvc;
using BeautySaloon.Models;
using BeautySaloon.Repositoryes.Search;

namespace BeautySaloon.Controllers
{
	public class SearchController : Controller
	{
		private readonly ISearchRepository _searchRepository;

		public SearchController(ISearchRepository searchRepository)
		{
			_searchRepository = searchRepository;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.State = true;
			var services = await _searchRepository.GetServices();
			var service = await _searchRepository.GetService();
			var model = new ViewModel { Service = service, Services = services };
            ViewData["State"] = ViewBag.State;
            return View(model);
		}

		public async Task<IActionResult> Search(string searchString)
		{
           

            var services = await _searchRepository.Search(searchString);
			return PartialView("SearchResult", services);
		}
	}
}
