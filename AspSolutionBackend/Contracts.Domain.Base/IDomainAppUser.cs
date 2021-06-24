using System;
using Microsoft.AspNetCore.Identity;

namespace Contracts.Domain.Base
{
    public interface IDomainAppUser<TAppUser> : IDomainAppUser<Guid, TAppUser>
        where TAppUser : IdentityUser<Guid>
    {
    }

    public interface IDomainAppUser<TKey, TAppUser>
        where TKey : IEquatable<TKey>
        where TAppUser : IdentityUser<TKey>
    {
        TAppUser? AppUser { get; set; }
    }
}