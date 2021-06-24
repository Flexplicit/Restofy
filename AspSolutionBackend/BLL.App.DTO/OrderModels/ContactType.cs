using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;
using BLL.App.DTO;
using BLL.App.DTO.OrderModels.DbEnums;

namespace BLL.App.DTO.OrderModels

{
    public class ContactType : DomainEntityId
    {

        [Required] [StringLength(30)] public EContactType TypeName { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }
}