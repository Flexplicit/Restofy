using System;
using System.ComponentModel.DataAnnotations;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Food : FoodEdit
    {
        public string FoodGroup { get; set; } = null!;
    }

    public class FoodEdit : FoodCreate
    {
        public Guid Id { get; set; }
        [Required] public Guid? CostId { get; set; }
    }

    public class FoodCreate
    {
        [Required] public Guid? FoodGroupId { get; set; }
        [Required] public Guid? RestaurantId { get; set; }
        [Required] public decimal CostWithVat { get; set; }
        [StringLength(20)] public string FoodName { get; set; } = null!;
        [StringLength(40)] public string? Description { get; set; }
        public string Picture { get; set; } = null!;
    }
}