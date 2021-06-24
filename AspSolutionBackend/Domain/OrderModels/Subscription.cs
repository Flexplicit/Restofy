using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.OrderModels.DbEnums;


namespace Domain.OrderModels
{
    public partial class Subscription : DomainEntityId
    {
        [Required] [StringLength(30)] public ESubscriptionType SubscriptionType { get; set; }
        public ICollection<RestaurantSubscription>? RestaurantSubscriptions { get; set; }

        [StringLength(100)] public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(5, 2)")] public decimal Cost { get; set; }

        public int ValidDayCount { get; set; }
    }
}