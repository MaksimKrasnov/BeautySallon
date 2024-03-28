using BeautySaloon.Email;
using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BeautySaloon.Controllers
{
	public class HomeController : Controller
	{
        ApplicationDbContext db;
        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            var service = await db.Service.Where(s => s.ServicesId == 1).ToListAsync();
            var services = await db.Services.ToListAsync();
            var model = new ViewModel { Service = service, Services = services };


            return View(model);
        }

       
        public async Task<IActionResult> GetServices(int id)
        {
            var service = await db.Service.Where(s => s.ServicesId == id).ToListAsync();
           
            return PartialView("PartView",service);
        }

      
        [HttpPost]
        public IActionResult SendMessage(string name, string phone)
        {
            EmailService email = new EmailService();
            email.SendMail("krasnovm020@gmail.com", "Перезвонить клиенту", $"Имя клиента: {name} телефон: {phone} ");
            return Ok();
        }
    }
}