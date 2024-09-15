using Microsoft.AspNetCore.Mvc;
using BeautySaloon.Models;
using BeautySaloon.Repositoryes.Home;

namespace BeautySaloon.Controllers
{
    public class HomeController : Controller
	{
		private readonly IHomeRepository homeRepository;

		public HomeController(IHomeRepository repository)
		{
			homeRepository = repository;
		}

		public async Task<IActionResult> Index()
		{
			var service = await homeRepository.GetServiceById(1);
			var services = await homeRepository.GetAllServices();
			var model = new ViewModel { Service = service, Services = services };

			return View(model);
		}

		public async Task<IActionResult> GetServices(int id)
		{
			var service = await homeRepository.GetServiceById(id);

			return PartialView("PartView", service);
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(string name, string phone)
		{
			await homeRepository.SendMessage(name, phone);
			return Ok();
		}
	}
}
