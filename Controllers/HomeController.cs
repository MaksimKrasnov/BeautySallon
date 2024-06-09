//using BeautySaloon.Email;
//using BeautySaloon.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Diagnostics;

//namespace BeautySaloon.Controllers
//{
//	public class HomeController : Controller
//	{
//        ApplicationDbContext db;
//        public HomeController(ApplicationDbContext context)
//        {
//            db = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var service = await db.Service.Where(s => s.ServicesId == 1).ToListAsync();
//            var services = await db.Services.ToListAsync();
//            var model = new ViewModel { Service = service, Services = services };


//            return View(model);
//        }


//        public async Task<IActionResult> GetServices(int id)
//        {
//            var service = await db.Service.Where(s => s.ServicesId == id).ToListAsync();

//            return PartialView("PartView",service);
//        }


//        [HttpPost]
//        public IActionResult SendMessage(string name, string phone)
//        {
//            EmailService email = new EmailService();
//            email.SendMail("krasnovm020@gmail.com", "Перезвонить клиенту", $"Имя клиента: {name} телефон: {phone} ");
//            return Ok();
//        }
//    }
//}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeautySaloon.Models;
using BeautySaloon.Repositoryes;
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
