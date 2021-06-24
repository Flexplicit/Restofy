using System;
using System.ComponentModel.DataAnnotations;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class PaymentType 
    {
        
        public Guid Id { get; set; }
        [StringLength(30)] public string Type { get; set; } = null!;
    }
}