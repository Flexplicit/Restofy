using Microsoft.AspNetCore.Mvc.Rendering;
using Contact = BLL.App.DTO.OrderModels.Contact;

#pragma warning disable 1591

namespace WebApp.ViewModels.Contacts
{
    public class ContactCreateEditViewModel
    {
        public Contact Contact { get; set; } = default!;

        public SelectList? ContactTypeSelectList { get; set; }
        public SelectList? UserSelectList { get; set; }
        public SelectList? RestaurantList { get; set; }
    }
}