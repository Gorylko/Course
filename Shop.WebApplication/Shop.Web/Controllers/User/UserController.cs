﻿using Shop.Business.Services;
using Shop.Shared.Entities.Authorize;
using Shop.Web.Attributes;
using Shop.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Shop.Web.Controllers
{
    public class UserController : Controller
    {
        private UserService _userService = new UserService();
        private ProductService _productService = new ProductService();

        public ActionResult ShowUsersList()
        {
            ViewBag.Users = _userService.GetAll();
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult ShowUsersList(SearchViewModel model)
        {
            if (model.Name == null)
            {
                ViewBag.Users = _userService.GetAll();
            }
            else
            {
                ViewBag.Users = _userService.GetAllByName(model.Name);
            }
            return View(model);
        }

        public ActionResult ShowUser(string login)
        {
            var user = _userService.GetByLogin(login);
            ViewBag.User = user;
            ViewBag.Title = user.Login;
            return View();
        }

        [User]
        public ActionResult ShowCurrentUser()
        {
            var currentUser = User as UserPrinciple;
            var user = _userService.GetByLogin(currentUser.Name);
            return View(new UserViewModel
            {
                Login = user.Login,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString()
            });
        }

        [User]
        public ActionResult Delete(int id)
        {
            ViewBag.Message = $"Пользователь \"{_userService.GetById(id).Login}\" удален успешно!";
            IReadOnlyCollection<Shared.Entities.Product> products = _productService.GetByUserId(id); 
            foreach(Shared.Entities.Product product in products)
            {
                _productService.DeleteById(product.Id);
            }
            _userService.DeleteById(id);
            return View("~/Views/Shared/Notification.cshtml");
        }
    }
}