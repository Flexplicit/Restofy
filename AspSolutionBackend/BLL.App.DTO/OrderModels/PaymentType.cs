using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;
using BLL.App.DTO;
using BLL.App.DTO.OrderModels.DbEnums;

namespace BLL.App.DTO.OrderModels

{
    public partial class PaymentType : DomainEntityId
    {
        // [Key] public Guid PaymentTypeId { get; set; }

        [Required]
        [StringLength(30)]
        public EPaymentType Type { get; set; }

        public  ICollection<Order>? Orders { get; set; }
    }
}