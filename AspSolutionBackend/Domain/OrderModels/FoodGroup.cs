using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.OrderModels.DbEnums;

namespace Domain.OrderModels
{
    public partial class FoodGroup : DomainEntityId
    {

        [Required]
        public FoodGroupType FoodGroupType { get; set; }

        public ICollection<Food>? Foods { get; set; }
    }
}