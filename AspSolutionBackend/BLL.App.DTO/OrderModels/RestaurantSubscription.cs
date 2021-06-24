using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using BLL.App.DTO;


namespace BLL.App.DTO.OrderModels

{
    public partial class RestaurantSubscription : DomainEntityId
    {
        public Guid SubscriptionId { get; set; }
        public Guid RestaurantId { get; set; }

        [StringLength(70)] public string PaypalId { get; set; } = null!;
        [Column(TypeName = "date")] public DateTime? ActiveSince { get; set; }
        [Column(TypeName = "date")] public DateTime? ActiveUntill { get; set; }


        public Restaurant? Restaurant { get; set; }
        public Subscription? Subscription { get; set; }
        public bool IsPaid { get; set; }
    }
}