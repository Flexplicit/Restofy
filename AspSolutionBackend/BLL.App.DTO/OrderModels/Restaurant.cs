using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;
using BLL.App.DTO;


namespace BLL.App.DTO.OrderModels

{
    public class Restaurant : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [Required] [StringLength(30)] public string Name { get; set; } = null!;

        [StringLength(255)] public string? Picture { get; set; }

        [Required] [StringLength(30)] public string RestaurantAddress { get; set; } = null!;
        [StringLength(100)] public string? Description { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
        public ICollection<Food>? RestaurantFood { get; set; }

        public ICollection<RestaurantSubscription>? RestaurantSubscriptions { get; set; }

        public ICollection<Order>? RestaurantOrder { get; set; }

        //Added in BLL
        public int SalesCount { get; set; }
        public bool IsValidSubscription { get; set; }
        public int SubscriptionDaysLeft { get; set; }

        // public TYPE Type { get; set; }


        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }
    }
}