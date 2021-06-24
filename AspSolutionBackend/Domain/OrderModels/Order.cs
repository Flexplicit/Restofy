using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;
using Domain.Base;
using Domain.Identity;
using Domain.OrderModels.DbEnums;


namespace Domain.OrderModels
{
    public class Order : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public Guid PaymentTypeId { get; set; }
        public Guid? RestaurantAddressId { get; set; }

        public Guid RestaurantId { get; set; }

        public Guid? CreditCardId { get; set; }

        public int OrderNumber { get; set; }

        public EOrderStatus OrderCompletionStatus { get; set; }

        [Column(TypeName = "datetime")] public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime")] public DateTime? ReadyAt { get; set; }

        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }

        public PaymentType? PaymentType { get; set; }

        public CreditCard? CreditCard { get; set; }

        public Restaurant? Restaurant { get; set; }

        public ICollection<Bill>? Bills { get; set; }

        public ICollection<FoodInOrder>? FoodInOrders { get; set; }
    }
}