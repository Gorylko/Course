﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Shop.Web.Models;
using Shop.Shared.Entities;
using Shop.Shared.Entities.Enums;
using Shop.Business.Services;
using System.Data.SqlClient;

namespace Shop.Web.Controllers
{
    public class AccountController : Controller
    {
        private UserService _userService = new UserService();


        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            Session["User"] = _userService.GetAuthorizedUser(model.Login, model.Password);
            if(Session["User"] == null)
            {
                ViewBag.ErrorMessage = "Ахтунг! Ашыбка, проверьте введенные данные";
                return View(model);
            }

            return Redirect("/Home/Index");
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Session["User"] = _userService.RegisterUser(model.Login, model.Password, model.Email, model.PhoneNumber);
            if (Session["User"] == null)
            {
                ViewBag.ErrorMessage = "Ахтунг! Ашыбка, проверьте введенные данные";
                return View(model);
            }

            return Redirect("/Home/Index");
        }

        public ActionResult OpenAccountMenu()
        {
            return View();
        }

        public ActionResult Exit()
        {
            Session["User"] = null;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}