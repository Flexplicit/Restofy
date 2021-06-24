using System;
using System.ComponentModel.DataAnnotations;
using PublicApiDTO.v1.v1.OrderModels.DbEnums;

namespace PublicApiDTO.v1.v1.OrderModels
{
    public class FoodGroup
    {
        public Guid Id { get; set; }

        [Required] public string FoodGroupType { get; set; } = null!;
    }
}