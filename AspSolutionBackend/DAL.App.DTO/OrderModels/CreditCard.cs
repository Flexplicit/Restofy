using System;
using System.Collections.Generic;
using Contracts.Domain.Base;
using DAL.App.DTO.Identity;
using Domain.Base;


namespace DAL.App.DTO.OrderModels
{
    public class CreditCard : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        
        public Guid CreditCardInfoId { get; set; }
        public CreditCardInfo? CreditCardInfo { get; set; }
        public AppUser? AppUser { get; set; }
        public Guid AppUserId { get; set; }

        
    }
}