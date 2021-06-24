using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;
using BLL.App.DTO;


namespace BLL.App.DTO.OrderModels

{
    public class Food : DomainEntityId
    {
        public Guid RestaurantId { get; set; }

        public Guid CostId { get; set; }

        public Guid FoodGroupId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(ResourceType = typeof(Resources.BLL.App.DTO.Food), Name = nameof(FoodName))]
        public string FoodName { get; set; } = null!;

        [StringLength(40)] public string? Description { get; set; }
        [Required] [StringLength(255)] public string Picture { get; set; } = null!;


        public Cost? Cost { get; set; }
        public FoodGroup? FoodGroup { get; set; }
        public Restaurant? Restaurant { get; set; }

        public ICollection<FoodInOrder>? FoodInOrders { get; set; }
    }
}