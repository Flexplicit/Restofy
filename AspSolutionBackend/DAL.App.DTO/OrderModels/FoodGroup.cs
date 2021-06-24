using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.OrderModels.DbEnums;
using Domain.Base;

namespace DAL.App.DTO.OrderModels
{
    public partial class FoodGroup : DomainEntityId
    {

        [Required]
        public FoodGroupType FoodGroupType { get; set; }

        public ICollection<Food>? Foods { get; set; }
    }
}