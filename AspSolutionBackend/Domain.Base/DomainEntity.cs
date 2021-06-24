using System;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class DomainEntity : DomainEntity<Guid>, IDomainEntity
    {
    }


    public class DomainEntity<TKey> : DomainEntityId<TKey>, IDomainEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public string CreatedBy { get; set; } = "system";
        public DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; } = "system";
        public DateTime UpdatedAt { get; set; }
    }
}