using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;


namespace Domain.OrderModels
{
    public class CreditCardInfo : DomainEntityId
    {
        // [Key] public Guid CreditCardInfoId { get; set; }

        [Required] [StringLength(16)] public string CreditCardNumber { get; set; } = null!;

        [Required] [StringLength(4)] public string ExpiryDate { get; set; } = null!;

        [Required] [StringLength(3)] public string Cvc { get; set; } = null!;

        public ICollection<CreditCard>? CreditCards { get; set; }
    }
}