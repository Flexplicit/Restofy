using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;


namespace Domain.OrderModels
{
    public partial class Food : DomainEntityId
    {
        // [Key] public Guid FoodId { get; set; }

        public Guid RestaurantId { get; set; }

        public Guid CostId { get; set; }

        public Guid FoodGroupId { get; set; }

        [Required] [StringLength(20)] public string FoodName { get; set; } = null!;
        [StringLength(40)] public string? Description { get; set; }
        [Required] [StringLength(255)] public string Picture { get; set; } = null!;


        public Cost? Cost { get; set; }
        public FoodGroup? FoodGroup { get; set; }
        public Restaurant? Restaurant { get; set; }

        public ICollection<FoodInOrder>? FoodInOrders { get; set; }
    }
}