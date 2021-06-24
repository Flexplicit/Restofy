using System;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class DomainEntityMeta : IDomainEntityMeta

    {
        public string CreatedBy { get; set; } = "system";
        public DateTime CreateAt { get; set; }
        public string UpdateBy { get; set; } = "system";
        public DateTime UpdatedAt { get; set; }
    }
}   