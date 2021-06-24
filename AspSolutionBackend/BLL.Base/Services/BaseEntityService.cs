using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

namespace BLL.Base.Services
{
    public class BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalDTOEntity>
        : BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalDTOEntity, Guid>,
            IBaseEntityService<TBllEntity, TDalDTOEntity>
        where TDalDTOEntity : class, IDomainEntityId
        where TBllEntity : class, IDomainEntityId
        where TUnitOfWork : IBaseUnitOfWork
        where TRepository : IBaseRepository<TDalDTOEntity>
    {
        public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
            IBaseMapper<TBllEntity, TDalDTOEntity> mapper)
            : base(serviceUow, serviceRepository, mapper)
        {
        }
    }

    public class
        BaseEntityService<TUnitOfWork, TRepository, TBllEntity, TDalDTOEntity, TKey>
        : IBaseEntityService<TBllEntity, TDalDTOEntity, TKey>
        where TDalDTOEntity : class, IDomainEntityId<TKey>
        where TBllEntity : class, IDomainEntityId<TKey>
        where TUnitOfWork : IBaseUnitOfWork
        where TRepository : IBaseRepository<TDalDTOEntity, TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly TUnitOfWork ServiceUow;
        protected readonly TRepository ServiceRepository;
        protected readonly IBaseMapper<TBllEntity, TDalDTOEntity> Mapper;


        public BaseEntityService(TUnitOfWork serviceUow, TRepository serviceRepository,
            IBaseMapper<TBllEntity, TDalDTOEntity> mapper)
        {
            ServiceUow = serviceUow;
            ServiceRepository = serviceRepository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TBllEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllAsync(userId, noTracking)).Select(entity => Mapper.Map(entity))!;
        }

        public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default, bool noTracking = true)
        {
            var dalEntity = await ServiceRepository.FirstOrDefaultAsync(id, userId, noTracking);
            var bllEntity = Mapper.Map(dalEntity);
            return bllEntity;
        }

        public async Task<bool> ExistsAsync(TKey id, TKey? userId = default)
        {
            return await ServiceRepository.ExistsAsync(id, userId);
        }

        public async Task<TBllEntity> RemoveAsync(TKey id, TKey? userId = default)
        {
            return Mapper.Map(await ServiceRepository.RemoveAsync(id, userId))!;
        }

        public TBllEntity Add(TBllEntity entity)
        {
            return Mapper.Map(ServiceRepository.Add(Mapper.Map(entity)!))!;
        }

        public  Task AddRangeAsync(IEnumerable<TBllEntity> entity)
        {
            throw new NotImplementedException();
            // await ServiceRepository.AddRangeAsync((entity.Select(x => Mapper.Map(x))!));
        }

        public void UpdateRange(IEnumerable<TBllEntity> entity)
        {
            throw new NotImplementedException();
        }

        public TBllEntity Update(TBllEntity entity)
        {
            return Mapper.Map(ServiceRepository.Update(Mapper.Map(entity)!))!;
        }

        public TBllEntity Remove(TBllEntity? entity, TKey? userId = default)
        {
            return Mapper.Map(ServiceRepository.Remove(Mapper.Map(entity)!))!;
        }
    }
}