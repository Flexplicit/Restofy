using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.OrderModels.DbEnums;
using Domain.Base;

namespace DAL.App.DTO.OrderModels
{
    public class ContactType : DomainEntityId
    {
        [Required] [StringLength(30)] public EContactType TypeName { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }
}