using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;
using Microsoft.EntityFrameworkCore;
// ReSharper disable SuspiciousTypeConversion.Global


namespace DAL.Base.EF.Repositories
{
    public class BaseRepository<TDalDTOEntity, TDomainEntity, TDbContext> :
        BaseRepository<TDalDTOEntity, TDomainEntity, Guid, TDbContext>, IBaseRepository<TDalDTOEntity>
        where TDalDTOEntity : class, IDomainEntityId
        where TDomainEntity : class, IDomainEntityId
        where TDbContext : DbContext
    {
        public BaseRepository(TDbContext ctx, IBaseMapper<TDalDTOEntity, TDomainEntity> mapper) : base(ctx, mapper)
        {
        }
    }

    public class BaseRepository<TDalDTOEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalDTOEntity, TKey>
        where TDalDTOEntity : class, IDomainEntityId<TKey>
        where TDomainEntity : class, IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext
    {
        protected readonly TDbContext RepoDbContext;
        protected readonly DbSet<TDomainEntity> RepoDbSet;
        protected readonly IBaseMapper<TDalDTOEntity, TDomainEntity> Mapper;


        public BaseRepository(TDbContext ctx, IBaseMapper<TDalDTOEntity, TDomainEntity> mapper)
        {
            RepoDbContext = ctx;
            Mapper = mapper;
            RepoDbSet = ctx.Set<TDomainEntity>();
        }

        protected IQueryable<TDomainEntity> CreateQuery(TKey? userId, bool noTracking)
        {
            var query = RepoDbSet.AsQueryable();

            if (userId != null && !userId.Equals(default) && typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                query = query.Where(entity => ((IDomainAppUserId<TKey>) entity).AppUserId.Equals(userId));
            }


            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public virtual async Task<IEnumerable<TDalDTOEntity>> GetAllAsync(TKey? userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return (await query.ToListAsync()).Select(x => Mapper.Map(x))!;
        }


        public virtual async Task<TDalDTOEntity?> FirstOrDefaultAsync(TKey id, TKey? userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return Mapper.Map(await query.FirstOrDefaultAsync(e => e.Id.Equals(id)));
        }

        public virtual async Task<bool> ExistsAsync(TKey id, TKey? userId)
        {
            if (userId == null || userId.Equals(default))
                // no ownership control, userId was null or default
                return await RepoDbSet.AnyAsync(e => e.Id.Equals(id));

            // we have userid and it is not null or default (null or 0) - so we should check for appuserid also
            // does the entity actually implement the correct interface
            if (!typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
                throw new AuthenticationException(
                    $"Entity {typeof(TDomainEntity).Name} does not implement required interface: {typeof(IDomainAppUserId<TKey>).Name} for AppUserId check");
            return await RepoDbSet.AnyAsync(e =>
                e.Id.Equals(id) && ((IDomainAppUserId<TKey>) e).AppUserId.Equals(userId));

        }

        public virtual TDalDTOEntity Add(TDalDTOEntity entity)
        {
            return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TDalDTOEntity> entity)
        {
            var domainsEntities = entity.Select(x => Mapper.Map(x));
            await RepoDbSet.AddRangeAsync(domainsEntities!);
        }

        public void UpdateRange(IEnumerable<TDalDTOEntity> entity)
        {
             RepoDbSet.UpdateRange(entity.Select(x=>Mapper.Map(x))!);
        }

        public virtual TDalDTOEntity Update(TDalDTOEntity entity)
        {
            
            return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
        }


        public virtual TDalDTOEntity Remove(TDalDTOEntity entity, TKey? userId)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (userId != null && !userId.Equals(default) &&
                typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)) &&
                !((IDomainAppUserId<TKey>) entity).AppUserId.Equals(userId))
            {
                throw new AuthenticationException(
                    $"Bad user id inside entity {typeof(TDalDTOEntity).Name} to be deleted.");
                // TODO: load entity from the db, check that userId inside entity is correct.
            }

            return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;

        }

        public virtual async Task<TDalDTOEntity> RemoveAsync(TKey id, TKey? userId = default)
        {
            var entity = await FirstOrDefaultAsync(id, userId);
            if (entity == null) throw new NullReferenceException($"Entity with id {id} not found.");
            return Remove(entity!, userId);
        }
    }
}