using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.OrderModels.DbEnums;
using Domain.Base;


namespace DAL.App.DTO.OrderModels
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