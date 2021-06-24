using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.App.DTO.OrderModels;
using Microsoft.AspNetCore.Identity;

namespace DAL.App.DTO.Identity
{
    public class AppUser : IdentityUser<Guid>
        // , IDomainEntityId
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string FirstName { get; set; } = null!;

        [StringLength(128, MinimumLength = 1)]
        [Required]
        public string LastName { get; set; } = null!;

        public DateTime Birthdate { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<CreditCard>? CreditCards { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }
}