using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class CreditCardInfo : CreditCardInfoCreate
    {
        public Guid Id { get; set; }
    }

    public class CreditCardInfoCreate
    {
        [Required] [StringLength(16)] public string CreditCardNumber { get; set; } = null!;
        [Required] [StringLength(4)] public string ExpiryDate { get; set; } = null!;
        [Required] [StringLength(3)] public string Cvc { get; set; } = null!;
    }
}