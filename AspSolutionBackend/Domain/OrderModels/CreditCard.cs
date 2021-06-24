using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;
using Domain.Base;
using Domain.Identity;


namespace Domain.OrderModels
{
    public class CreditCard : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        
        public Guid CreditCardInfoId { get; set; }


        public CreditCardInfo? CreditCardInfo { get; set; }

        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}