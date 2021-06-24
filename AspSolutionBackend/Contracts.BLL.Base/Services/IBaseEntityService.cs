using System;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

namespace Contracts.BLL.Base.Services
{
    
    
    public interface IBaseEntityService<TBllEntity,TDalDTOEntity> : IBaseEntityService<TBllEntity,TDalDTOEntity, Guid>
        where TDalDTOEntity : class, IDomainEntityId
        where TBllEntity : class, IDomainEntityId
    {
    }


    public interface IBaseEntityService<TBllEntity,TDalDTOEntity, TKey> : IBaseService, IBaseRepository<TBllEntity, TKey>
        where TKey : IEquatable<TKey>
        where TDalDTOEntity : class, IDomainEntityId<TKey>
        where TBllEntity : class, IDomainEntityId<TKey>
    {
    }
}