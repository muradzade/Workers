using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Workers.Data;
using Workers.Data.Entity;
using Workers.WebUI.Models;

namespace Workers.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string sortBy,string sortParam,
                                                string search)
        {
            IEnumerable<Worker> workers = _db.Workers;
            ViewBag.SearchBox = search;
           
            if(!string.IsNullOrEmpty(search))
            {
                workers = workers.Where(w => w.Name.ToLower().Contains(search.ToLower()) ||
                                         w.Surname.ToLower().Contains(search.ToLower()) ||
                                         w.Phone.Contains(search));
            }

            if(sortParam == "name")
            {
                switch (sortBy)
                {
                    case "desc_name":
                        ViewBag.sortByName = "asc_name";
                        workers = workers.OrderBy(w => w.Name).ToList();
                        break;
                    case "asc_name":
                        ViewBag.sortByName = "desc_name";
                        workers = workers.OrderByDescending(w => w.Name).ToList();
                        break;
                    default:
                        ViewBag.sortByName = "desc_name";
                        workers = workers.OrderByDescending(w => w.Name).ToList();
                        break;

                }
            }
            else if(sortParam == "surname")
            {
                switch (sortBy)
                {
                    case "asc_surname":
                        ViewBag.sortBySurname = "desc_surname";
                        workers = workers.OrderByDescending(w => w.Surname).ToList();
                        break;
                    case "desc_surname":
                        ViewBag.sortBySurname = "asc_surname";
                        workers = workers.OrderBy(w => w.Surname).ToList();
                        break;
                    default:
                        ViewBag.sortBySurname = "desc_surname";
                        workers = workers.OrderByDescending(w => w.Surname).ToList();
                        break;
                }
            }
            else
            {
                ViewBag.sortByName = "asc_name";
                workers = workers.OrderBy(w => w.Name).ToList();
            }
            
            return View(workers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
