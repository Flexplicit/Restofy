using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.OrderModels.DbEnums;


namespace Domain.OrderModels
{
    public class ContactType : DomainEntityId
    {
        [Required] [StringLength(30)] public EContactType TypeName { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }
}