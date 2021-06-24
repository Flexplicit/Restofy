using System;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string SubscriptionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        public int ValidDayCount { get; set; }
    }
}