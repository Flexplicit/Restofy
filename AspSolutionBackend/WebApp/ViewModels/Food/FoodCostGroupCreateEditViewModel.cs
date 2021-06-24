using Microsoft.AspNetCore.Mvc.Rendering;
#pragma warning disable 1591

namespace WebApp.ViewModels.Food
{
    public class FoodCostGroupCreateEditViewModel
    {
        public BLL.App.DTO.OrderModels.Food? Food { get; set; }

        // public Cost Cost { get; set; } = default!;
        public SelectList? FoodGroupSelectList { get; set; }
        public SelectList? RestaurantsSelectList { get; set; }
    }
}