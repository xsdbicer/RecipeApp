﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeApp.Core.Repositories.Interfaces;
using RecipeApp.Core.Services.Interfaces;
using RecipeApp.Core.ViewModels.OutViewModels;
using RecipeApp.Service.Services.Objects;
using System.Security.Claims;

namespace RecipeApp.Web.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IRecipeService _recipeService;

        public FavoriteController(IFavoriteService favoriteService, IRecipeService recipeService)
        {
            _favoriteService = favoriteService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _favoriteService.GetAllFavoritesWithRecipe());
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int recipeId, string userId)
        {
            //TODO: Favorilere eklendiğinde UI'daki Favorilere ekle buton adının değişmesi gerekiyor. 
            //TODO: Favoriler kişiye özel değil. bug
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            var user = userId;
            //TODO burada koşullu yapıyı düzenleyerek daha sade hale getirebilirim. 
            if (recipe == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var favorite = _favoriteService.GetFavoriteWithRecipeAndUser(recipeId , userId);
                if(favorite == null)
                {
                    var favoriteVM = new FavoriteOutVM
                    {
                        RecipeId = recipe.Id,
                        UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                    };
                    var favorites = await _favoriteService.AddAsync(favoriteVM);

                    TempData["Message"] = "Favorilerinize Eklendi!";
                }
                else
                {
                    TempData["Message"] = "Tarif zaten favorilerinizde!";
                }
            }
            //TODO: Şurada favorilerin index'ine değil de recipe controller index'e yönlendirmem lazım.
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Remove(int id)
        {
            var fav =await _favoriteService.GetByIdAsync(id);
            await _favoriteService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
