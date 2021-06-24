using System.Collections;
using System.Collections.Generic;
#pragma warning disable 1591

namespace WebApp.ViewModels.Restaurant
{

    public class RestaurantIndexViewModel
    {
        public ICollection<BLL.App.DTO.OrderModels.Restaurant> Restaurants { get; set; } = null!;

        public bool IsRestaurantOwnerView { get; set; } = false;
    }
}