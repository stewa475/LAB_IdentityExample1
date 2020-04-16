﻿using System;
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
            ViewData["tasks"] = dal.GetTasksAll();
            ViewData["Name"] = User.Identity.Name;
            ViewData["UID"] = _userManager.GetUserId(User);
            return View("TaskIndex");
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

            int loginId = int.Parse(_userManager.GetUserId(User));

            //add to the database here
            if (ModelState.IsValid)
            {
                int result = dal.CreateTask(t, loginId);
            }
            else
            {
                ViewData["errorMsg"] = "Your form had errors. Please correct and re-submit.";
                return View("AddTask", t);
            }

            return View("TaskIndex");
        }

        public IActionResult Delete(int id)
        {
            int result = dal.DeleteTasksById(id);

            if (result == 1)
            {
                TempData["UserMsg"] = "Item successfully deleted";
            }
            else
            {
                TempData["UserMsg"] = "Item for deletion not found";
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
                TempData["UserMsg"] = "Item successfully updated";
            }
            else
            {
                TempData["UserMsg"] = "Item not updated";
            }

            return RedirectToAction("Index");
        }
    }
}