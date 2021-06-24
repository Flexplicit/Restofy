using System;
using System.ComponentModel.DataAnnotations;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Restaurant : RestaurantEdit
    {
        //Added in BLL
        public int SalesCount { get; set; }
        public bool IsValidSubscription { get; set; }
        public int SubscriptionDaysLeft { get; set; }
    }


    public class RestaurantEdit : RestaurantCreate
    {
        public Guid Id { get; set; }
    }


    public class RestaurantCreate
    {
        [Required] [StringLength(30)] public string Name { get; set; } = null!;
        [Required] [StringLength(30)] public string RestaurantAddress { get; set; } = null!;
        public string? Picture { get; set; } // base64
        [StringLength(100)] public string? Description { get; set; }
    }
}