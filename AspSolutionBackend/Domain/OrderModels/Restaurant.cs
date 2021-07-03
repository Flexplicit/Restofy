using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Domain.Base;
using Domain.Identity;


namespace Domain.OrderModels
{
    public sealed class Restaurant : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        // [Required] [StringLength(30)] public string Name { get; set; } = null!;
        public LangString? NameLang { get; set; }
        public Guid NameLangId { get; set; }


        public LangString? DescriptionLang { get; set; }
        public Guid? DescriptionLangId { get; set; }
        [StringLength(255)] public string? Picture { get; set; }

        [Required] [StringLength(30)] public string RestaurantAddress { get; set; } = null!;

        // [StringLength(100)] public string? Description { get; set; }

        public ICollection<Contact>? Contacts { get; set; }


        public ICollection<Food>? RestaurantFood { get; set; }

        public ICollection<RestaurantSubscription>? RestaurantSubscriptions { get; set; }

        public ICollection<Order>? RestaurantOrder { get; set; }
        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }
    }
}