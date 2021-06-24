using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;
using BLL.App.DTO;
using BLL.App.DTO.OrderModels.DbEnums;


namespace BLL.App.DTO.OrderModels

{
    public class Order : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public Guid PaymentTypeId { get; set; }
        public Guid AppUserId { get; set; }
        
        public Guid? RestaurantAddressId { get; set; }
        public Guid RestaurantId { get; set; }

        public Guid? CreditCardId { get; set; }
        public int OrderNumber { get; set; }

        public EOrderStatus OrderCompletionStatus { get; set; }

        [Column(TypeName = "datetime")] public DateTime? CreatedAt { get; set; }

        [Column(TypeName = "datetime")] public DateTime? ReadyAt { get; set; }

        public AppUser? AppUser { get; set; }

        
        //Added in BLL
        public bool IsConfirmedByRestaurant { get; set; }
        public bool IsConfirmedByAppUser { get; set; }
        public decimal OrderTotalCost { get; set; }
        
        
    }
}