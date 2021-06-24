using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace Domain.OrderModels
{
    public sealed class FoodInOrder : DomainEntityId
    {
        public Guid? OrderId { get; set; }
        public Guid? FoodId { get; set; }

        public int Amount { get; set; }

        public Food? Food { get; set; }
        public Order? Order { get; set; }
    }
}