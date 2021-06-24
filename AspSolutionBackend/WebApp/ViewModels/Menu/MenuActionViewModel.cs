using System.Collections.Generic;

#pragma warning disable 1591

namespace WebApp.ViewModels.Menu
{
    public class MenuActionViewModel
    {
        public BLL.App.DTO.OrderModels.Restaurant Restaurant { get; set; } = null!;

        public ICollection<BLL.App.DTO.OrderModels.Food> Food { get; set; } = null!;

        public ICollection<BLL.App.DTO.OrderModels.Contact> Socials { get; set; } = null!;
        public bool IsRestaurantOwner { get; set; }
    }
}