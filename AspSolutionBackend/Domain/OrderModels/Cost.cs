using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;


namespace Domain.OrderModels
{
    public class Cost : DomainEntityId
    {
        // [Key] public Guid CostId { get; set; }


        [Column(TypeName = "decimal(10, 2)")] public decimal CostWithVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")] public decimal CostWithoutVat { get; set; }
        [Column(TypeName = "decimal(1, 1)")] public decimal Vat { get; set; }


        public ICollection<Food>? Foods { get; set; }
    }
}