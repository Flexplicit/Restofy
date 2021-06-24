using System;
using Domain.Base;

namespace DAL.App.DTO.OrderModels
{
    public sealed class FoodInOrder : DomainEntityId
    {
        // [Key] public Guid FoodInOrderId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid FoodId { get; set; }

        public int Amount { get; set; }

        public Food? Food { get; set; }
        public Order? Order { get; set; }
    }
}