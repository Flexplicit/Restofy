using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;
using BLL.App.DTO.OrderModels.DbEnums;

namespace BLL.App.DTO.OrderModels

{
    public partial class FoodGroup : DomainEntityId
    {

        [Required]
        public FoodGroupType FoodGroupType { get; set; }

        public ICollection<Food>? Foods { get; set; }
    }
}