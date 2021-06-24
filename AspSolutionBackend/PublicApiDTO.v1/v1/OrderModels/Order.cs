using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PublicApiDTO.v1.v1.OrderModels.DbEnums;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid PaymentTypeId { get; set; }

        public Guid? CreditCardId { get; set; }
        
        public Guid RestaurantId { get; set; }

        public int OrderNumber { get; set; }
        public EOrderStatus OrderCompletionStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ReadyAt { get; set; }

        //Added in BLL
        public int? BillCount { get; set; }
        public bool IsConfirmedByRestaurant { get; set; }
        public bool IsConfirmedByAppUser { get; set; }
        public decimal OrderTotalCost { get; set; }
    }

    
    
    public class OrderCreate
    {
        public IEnumerable<Guid> FoodInOrderId { get; set; } = null!;
        public Guid PaymentTypeId { get; set; }
        public Guid? CreditCardId { get; set; }
    }

    public class OrderConfirmRestaurant
    {
        public Guid OrderId { get; set; }
        public int MinutesTillReady { get; set; }
    }
}