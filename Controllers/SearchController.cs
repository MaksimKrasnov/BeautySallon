using BeautySaloon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext db;

        public SearchController(ApplicationDbContext context)
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
        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View("Index", null);
            }

            List<Service> service = db.Service
                .Where(ap => ap.Name.Contains(searchString))
                .ToList();

            return PartialView("SearchResult", service);
        }
    }
  }
