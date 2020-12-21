using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workers.Data;
using Workers.Data.Entity;
using Workers.WebUI.Models;

namespace Workers.WebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Calisanlar
        [Route("/Admin/Workers")]
        public IActionResult ListWorkers(string sortBy,string sortParam,
                                            string search)
        {
            IEnumerable<Worker> workers = _db.Workers;
            foreach(var worker in workers)
            {
                worker.Manager = _db.Workers.FirstOrDefault(m => m.Id == worker.ManagerId);
                worker.Department = _db.Departments.FirstOrDefault(d => d.Id == worker.DeptId);
                if(worker.Manager==null)
                    worker.Manager = new Worker() { Name = "Yönetici atanmamış" };

                if (worker.Department == null)
                    worker.Department = new Department() { DeptName = "Departman atanmamış" };
            }

            if(!string.IsNullOrEmpty(search))
            {
                ViewBag.SearchBox = search;
                workers = workers.Where(w => w.Name.ToLower().Contains(search.ToLower()) ||
                                        w.Surname.ToLower().Contains(search.ToLower()) ||
                                        w.Phone.Contains(search));
            }

            if(string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortParam))
            {
                sortParam = "name";
                sortBy = "desc";
            }
            switch(sortParam)
            {
                case "name":
                    switch(sortBy)
                    {
                        case "asc":
                            ViewBag.sortBy = "desc";
                            workers = workers.OrderByDescending(w => w.Name);
                            break;
                        case "desc":
                            ViewBag.sortBy = "asc";
                            workers = workers.OrderBy(w => w.Name);
                            break;
                    }break;
                    
                case "surname":
                    switch (sortBy)
                    {
                        case "asc":
                            ViewBag.sortBy = "desc";
                            workers = workers.OrderByDescending(w => w.Surname);
                            break;
                        case "desc":
                            ViewBag.sortBy = "asc";
                            workers = workers.OrderBy(w => w.Surname);
                            break;
                    }break;
                    
                case "department":
                    switch (sortBy)
                    {
                        case "asc":
                            ViewBag.sortBy = "desc";
                            workers = workers.OrderByDescending(w => w.Department.DeptName);
                            break;
                        case "desc":
                            ViewBag.sortBy = "asc";
                            workers = workers.OrderBy(w => w.Department.DeptName);
                            break;
                    }break;
                    
                case "manager":
                    switch (sortBy)
                    {
                        case "asc":
                            ViewBag.sortBy = "desc";
                            workers = workers.OrderByDescending(w => w.Manager.Name);
                            break;
                        case "desc":
                            ViewBag.sortBy = "asc";
                            workers = workers.OrderBy(w => w.Manager.Name);
                            break;
                    }break;
            }

            return View(workers.ToList());
        }

        [HttpGet]
        public IActionResult AddWorker()
        {
            ViewBag.Departments = _db.Departments.ToList();
            ViewBag.Managers = _db.Workers.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddWorker(Worker worker)
        {
            if (ModelState.IsValid)
            {
                _db.Workers.Add(worker);
                _db.SaveChanges();
            }
            return Redirect("/Admin/Workers");
        }

        [HttpGet]
        public IActionResult EditWorker(int id)
        {
            var worker = _db.Workers.FirstOrDefault(w => w.Id == id);
            ViewBag.Departments = _db.Departments.ToList();
            ViewBag.Managers = _db.Workers.ToList();
            return View(worker);
        }

        [HttpPost]
        public IActionResult EditWorker(Worker worker)
        {
            if(ModelState.IsValid)
            {
                _db.Entry(worker).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return Redirect("/Admin/Workers");
        }

        [HttpGet]
        public IActionResult DeleteWorker(int id)
        {
            var worker = _db.Workers.FirstOrDefault(w => w.Id == id);
            worker.Manager = _db.Workers.FirstOrDefault(m => m.Id == worker.ManagerId);
            worker.Department = _db.Departments.FirstOrDefault(d => d.Id == worker.DeptId);
            return View(worker);
        }
        [HttpPost]
        public IActionResult DeleteWorker(Worker worker)
        {
            var result = _db.Workers.Any(w => w.ManagerId == worker.Id);
            if (result)
            {
                ModelState.AddModelError(nameof(worker), "Bu çalışan yönetici olduğu için silinemez");
                return View(worker);
            }
            _db.Workers.Remove(worker);
            _db.SaveChanges();
            return Redirect("/Admin/Workers");
        }

        //Departmanlar
        [Route("/Admin/Departments")]
        public IActionResult ListDepartments(string sortBy,string search)
        {
            IEnumerable<Department> departments = _db.Departments;
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.SearchBox = search;
                departments = departments.Where(d=>d.DeptName.ToLower().Contains(search.ToLower()));
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "desc";
            }
            switch (sortBy)
            {
                case "asc":
                    ViewBag.sortBy = "desc";
                    departments = departments.OrderByDescending(d => d.DeptName);
                    break;

                case "desc":
                    ViewBag.sortBy = "asc";
                    departments = departments.OrderBy(d => d.DeptName);
                    break;

            }

            return View(departments.ToList());
        }

        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDepartment(Department dept)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Add(dept);
                _db.SaveChanges();
            }
            return Redirect("/Admin/Departments");
        }
        [HttpGet]
        public IActionResult EditDepartment(int id)
        {          
            return View(_db.Departments.FirstOrDefault(d => d.Id == id));
        }
        [HttpPost]
        public IActionResult EditDepartment(Department dept)
        {
            if(ModelState.IsValid)
            {
                _db.Entry(dept).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return Redirect("/Admin/Departments");
        }
        [HttpGet]
        public IActionResult DeleteDepartment(int id)
        {
            return View(_db.Departments.FirstOrDefault(d => d.Id == id));
        }
        [HttpPost]
        public IActionResult DeleteDepartment(Department dept)
        {
            var result = _db.Workers.Any(w => w.DeptId == dept.Id);
            if(result)
            {
                ModelState.AddModelError(nameof(dept), "Bu departmanda çalışanlar olduğu için silinemez");
                return View(dept);
            }
            _db.Departments.Remove(dept);
            _db.SaveChanges();
            return Redirect("/Admin/Departments");
        }
    }
}