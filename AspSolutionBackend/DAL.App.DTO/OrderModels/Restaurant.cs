using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using DAL.App.DTO.Identity;
using Domain.Base;


namespace DAL.App.DTO.OrderModels
{
    public sealed class Restaurant : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        [Required] [StringLength(30)] public string Name { get; set; } = null!;

        [StringLength(255)] public string? Picture { get; set; }

        [StringLength(100)] public string? Description { get; set; }

        [Required] [StringLength(30)] public string RestaurantAddress { get; set; } = null!;

        public ICollection<Contact>? Contacts { get; set; }

        public ICollection<Food>? RestaurantFood { get; set; }

        public ICollection<RestaurantSubscription>? RestaurantSubscriptions { get; set; }

        public ICollection<Order>? RestaurantOrder { get; set; }

        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }
    }
}