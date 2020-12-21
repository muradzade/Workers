using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Workers.Data;
using Workers.Data.Entity;

namespace Workers.WebUI.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public WorkerController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            Worker worker = _db.Workers.FirstOrDefault(w => w.Id == id);
            worker.Manager = _db.Workers.FirstOrDefault(w => w.Id == worker.ManagerId);
            worker.Department = _db.Departments.FirstOrDefault(d => d.Id == worker.DeptId);
            return View(worker);
        }
    }
}