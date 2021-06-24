using System;
using System.Collections.Generic;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;
using BLL.App.DTO;

namespace BLL.App.DTO.OrderModels

{
    public class CreditCard
        : DomainEntityId, IDomainAppUserId, IDomainAppUser<AppUser>
    {
        public Guid CreditCardInfoId { get; set; }
        public CreditCardInfo? CreditCardInfo { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}