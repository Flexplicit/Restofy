using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Domain.OrderModels;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser : IdentityUser<Guid>
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