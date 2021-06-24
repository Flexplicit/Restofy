using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicApiDTO.v1.v1.OrderModels
{
    public partial class RestaurantSubscription : RestaurantSubscriptionCreate
    {
        public Guid Id { get; set; }
        [Column(TypeName = "date")] public DateTime? ActiveSince { get; set; }
        [Column(TypeName = "date")] public DateTime? ActiveUntil { get; set; }
    }

    public class RestaurantSubscriptionCreate
    {
        public Guid SubscriptionId { get; set; }

        public string PaypalId { get; set; } = null!;
        public Guid RestaurantId { get; set; }
        public bool IsPaid { get; set; }
    }
}