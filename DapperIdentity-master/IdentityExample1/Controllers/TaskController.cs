using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Dapper.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityExample1.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityExample1.Controllers
{

    public class TaskController : Controller
    {
        private readonly UserManager<DapperIdentityUser> _userManager;
        private readonly SignInManager<DapperIdentityUser> _signInManager;
        IConfiguration ConfigRoot;
        DAL dal;

        public TaskController(
            UserManager<DapperIdentityUser> userManager,
            SignInManager<DapperIdentityUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            ConfigRoot = config;
            dal = new DAL(ConfigRoot.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
                
            if (User.Identity.Name != null)
            {
                int id = int.Parse(_userManager.GetUserId(User));
                ViewData["tasks"] = dal.GetTasksByUserId(id);
                ViewData["Name"] = User.Identity.Name;
                ViewData["UID"] = _userManager.GetUserId(User);
                return View("TaskIndex");
            }
            else
            {
                return View("../Account/Login");
            }
            
        }

        [HttpGet]
        public IActionResult AddTask()
        {
            ViewData["Name"] = User.Identity.Name;
            ViewData["UID"] = _userManager.GetUserId(User);
            if (User.Identity.Name != null)
            {
                return View(new Tasks());
            }
            else
            {
                return View("../Account/Login");
            }
        }

        [HttpPost]
        public IActionResult AddTask(Tasks t)
        {
            //add to the database here
            if (ModelState.IsValid)
            {
                int loginId = int.Parse(_userManager.GetUserId(User));
                int result = dal.CreateTask(t, loginId);
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["errorMsg"] = "Your form had errors. Please correct and re-submit.";
                return View("AddTask", t);
            }
            
        }

        public IActionResult Delete(int id)
        {
            int result = dal.DeleteTasksById(id);

            if (result == 1)
            {
                TempData["UserMsg"] = "Task successfully deleted";
            }
            else
            {
                TempData["UserMsg"] = "Task for deletion not found";
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteTasks(int id)
        {
            Tasks t = dal.GetTasksById(id);

            ViewData["Title"] = t.TaskTitle;
            ViewData["Tasks"] = t;
            ViewData["UID"] = _userManager.GetUserId(User);

            return View(t);
        }

        [HttpGet]
        public IActionResult EditTasks(int id)
        {
            Tasks t = dal.GetTasksById(id);

            if (t == null)
            {
                return View("NoSuchItem");
            }
            else
            {
                return View(t);
            }
        }


        [HttpPost]
        public IActionResult Complete(int id)
        {
            var task = dal.GetTasksById(id);
            task.Complete = !task.Complete;
            dal.UpdateTasksById(task);
            return Index();
        }

        [HttpPost]
        public IActionResult EditTasks(Tasks t)
        {
            int result;

            if (ModelState.IsValid)
            {
                result = dal.UpdateTasksById(t);
                
            }
            else
            {
                ViewData["errorMsg"] = "Your form had errors. Please correct and re-submit.";
                return View(t);
            }

            if (result == 1)
            {
                TempData["UserMsg"] = "Task successfully updated";
            }
            else
            {
                TempData["UserMsg"] = "Task not updated";
            }

            return RedirectToAction("Index", result);
        }

        public IActionResult Search(string search)
        {
            IEnumerable<Tasks> results = dal.Search(search);

            ViewData["Search Results"] = results;

            return View();
        }

        public IActionResult Detail(int id)
        {
            Tasks t = dal.GetTasksById(id);

            if (t == null)
            {
                return View("NoSuchItem");
            }
            else
            {
                ViewData["Title"] = t.TaskTitle;
                ViewData["Tasks"] = t;

                return View();
            }
        }
    }
}