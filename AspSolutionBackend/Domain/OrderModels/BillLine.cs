using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;


namespace Domain.OrderModels
{
    public class BillLine : DomainEntityId
    {
        public Guid BillId { get; set; }

        public int Amount { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceMultipliedWithAmountWithoutVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceMultipliedWithAmountWithVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PiecePrice { get; set; }

        public Bill Bill { get; set; } = null!;
    }
}
