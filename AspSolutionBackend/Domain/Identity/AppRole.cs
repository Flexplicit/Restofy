using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        // [StringLength(128, MinimumLength = 1)]
        // public string DisplayName { get; set; } = null!;
    }
}