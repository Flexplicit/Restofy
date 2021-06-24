using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Bill
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalCostWithoutVat { get; set; }
        public decimal TotalCostWithVat { get; set; }
    }
}