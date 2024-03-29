﻿using AspNetCore.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Core.Models;
using RecipeApp.Web.Models;
using RecipeApp.Core.ViewModels;
using System.Diagnostics;
using RecipeApp.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RecipeApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRecipeService _recipeService;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IRecipeService recipeService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _recipeService = recipeService;
        }

        public IActionResult Index()
        {
            return View(_recipeService.GetAllRecipesInOrderByDateDesc());
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

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user=await _userManager.CreateAsync(new()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName=model.FullName
            }, model.PasswordConfirmed);
            if (user.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarılı.";
                return Redirect(nameof(HomeController.Index));
            }
            foreach(var item in user.Errors) 
            {
                ModelState.AddModelError(string.Empty,item.Description);
            }
            return View();
        }

        
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var HasUser= await _userManager.FindByEmailAsync(model.Email);
            if (HasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış.");
                return View();

            }

            var result =await _signInManager.PasswordSignInAsync(HasUser, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış" });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string SearchString)
        {
            return View(await _recipeService.GetRecipeWithFilter(SearchString));
        }
       

    }
}