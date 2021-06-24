using System;
using Microsoft.AspNetCore.Identity;

namespace PublicApiDTO.v1.v1.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        // [StringLength(128, MinimumLength = 1)]
        // public string DisplayName { get; set; } = null!;
    }
}