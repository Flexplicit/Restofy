using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;

namespace DAL.App.DTO.OrderModels
{
    public class BillLine : DomainEntityId
    {
        public Guid BillId { get; set; }
        public string Name { get; set; } = null!;
        public int Amount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceMultipliedWithAmountWithoutVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceMultipliedWithAmountWithVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PiecePrice { get; set; }

        public Bill Bill { get; set; } = null!;
    }
}
