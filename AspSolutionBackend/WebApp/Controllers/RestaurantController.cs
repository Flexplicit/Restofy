using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels.Menu;
using WebApp.ViewModels.Restaurant;

#pragma warning disable 1591

namespace WebApp.Controllers
{
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IAppBLL _bll;

        public RestaurantController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Restaurant
        public async Task<IActionResult> Index()
        {
            return View(new RestaurantIndexViewModel()
            {
                IsRestaurantOwnerView = false,
                Restaurants = (await _bll.Restaurants.GetAllAsync()).ToList()
            });
        }

        public async Task<IActionResult> RestaurantMenu(Guid restaurantId)
        {
            var resWithMenu = await _bll.Restaurants.GetRestaurantWithMenuAsync(restaurantId);
            return View(new MenuActionViewModel()
            {
                Restaurant = resWithMenu!,
                Food = resWithMenu!
                    .RestaurantFood!
                    .OrderBy(x => x.FoodGroup!.FoodGroupType!)
                    .ToList(),
                IsRestaurantOwner = resWithMenu.AppUserId.Equals(User.GetUserId()!.Value),
                Socials = resWithMenu.Contacts!
            });
        }

        // GET: Restaurant/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _bll.Restaurants.GetRestaurantWithMenuAsync(id.Value, User.GetUserId()!.Value);

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurant/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BLL.App.DTO.OrderModels.Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                restaurant.AppUserId = User.GetUserId()!.Value;
                _bll.Restaurants.Add(restaurant);
                await _bll.SaveChangesTask();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurant/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _bll.Restaurants.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value);
            if (restaurant == null)
            {
                return NotFound();
            }

            if (restaurant.AppUserId != User.GetUserId()!.Value)
            {
                return Unauthorized();
            }

            return View(restaurant);
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, BLL.App.DTO.OrderModels.Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }
            // TODO: quick check if User is owner of restaurant.

            if (!ModelState.IsValid || !await _bll.Restaurants.ExistsAsync(id, User.GetUserId()!.Value))
            {
                return View(restaurant);
            }


            restaurant.AppUserId = User.GetUserId()!.Value;
            _bll.Restaurants.Update(restaurant);
            await _bll.SaveChangesTask();

            return RedirectToAction(nameof(Index));
        }


        // GET: Restaurant/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _bll.Restaurants.FirstOrDefaultAsync(id.Value);
            if (restaurant == null)
            {
                return NotFound();
            }

            if (restaurant.AppUserId != User.GetUserId()!.Value)
            {
                return Unauthorized();
            }

            return View(restaurant);
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Restaurants.RemoveAsync(id, User.GetUserId()!.Value);
            await _bll.SaveChangesTask();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> MyRestaurants()
        {
            return View(new RestaurantIndexViewModel()
            {
                IsRestaurantOwnerView = true,
                Restaurants = (await _bll.Restaurants.GetMyRestaurantsAsync(User.GetUserId()!.Value)).ToList()
            });
        }


        private async Task<bool> RestaurantExists(Guid id)
        {
            return await _bll.Restaurants.ExistsAsync(id, User.GetUserId()!.Value);
        }
    }
}