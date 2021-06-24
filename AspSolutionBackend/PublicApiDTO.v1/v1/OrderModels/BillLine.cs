using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class BillLine
    {
        public Guid Id { get; set; }
        public Guid BillId { get; set; }
        public string Name { get; set; } = null!;
        public int Amount { get; set; }
        public decimal PriceMultipliedWithAmountWithoutVat { get; set; }
        public decimal PriceMultipliedWithAmountWithVat { get; set; }
        public decimal PiecePrice { get; set; }
    }
}