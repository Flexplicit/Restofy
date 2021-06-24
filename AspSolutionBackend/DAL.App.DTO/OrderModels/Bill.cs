using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;


namespace DAL.App.DTO.OrderModels
{
    public sealed class Bill : DomainEntityId

    {
        public Guid OrderId { get; set; }
        public Guid CreditCardId { get; set; }

        [Column("total_cost_without_VAT", TypeName = "decimal(10, 2)")]
        public decimal TotalCostWithoutVat { get; set; }

        [Column("total_cost_with_VAT", TypeName = "decimal(10, 2)")]
        public decimal TotalCostWithVat { get; set; }


        public CreditCard? CreditCard { get; set; }
        public Order? Order { get; set; }
        
        public ICollection<BillLine>? BillLines { get; set; }
    }
}