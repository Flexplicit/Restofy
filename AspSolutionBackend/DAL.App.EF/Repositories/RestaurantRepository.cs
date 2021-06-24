using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories
{
    public class RestaurantRepository :
        BaseRepository<DalAppDTO.Restaurant, Domain.OrderModels.Restaurant, AppDbContext>, IRestaurantRepository
    {
        public RestaurantRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new RestaurantMapper(mapper))
        {
        }


        public void GetTopRestaurants()
        {
            throw new NotImplementedException();
        }


        public override DalAppDTO.Restaurant Remove(DalAppDTO.Restaurant entity, Guid userId)
        {
            return base.Remove(entity, userId);
        }

        public override async Task<DalAppDTO.Restaurant?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            var domainEntity = await query
                .Include(x => x.AppUser)
                .Include(x => x.RestaurantSubscriptions)
                .Where(entity => entity.Id == id)
                .FirstOrDefaultAsync();
            var dalEntity = Mapper.Map(domainEntity);
            return dalEntity;
        }

        public override async Task<IEnumerable<DalAppDTO.Restaurant>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            return (await query
                .Include(x => x!.RestaurantSubscriptions)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }


        public async Task<DalAppDTO.Restaurant?> GetRestaurantWithMenuAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return Mapper.Map(await query
                .Include(x => x.RestaurantFood)
                .ThenInclude(x => x.Cost)
                .Include(x => x.RestaurantFood)
                .ThenInclude(x => x.FoodGroup)
                .Include(x => x.Contacts)
                .Include(x => x.RestaurantSubscriptions)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }

        public async Task<IEnumerable<DalAppDTO.Restaurant>?> GetMyRestaurants(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            return
                (await query
                    .Include(x => x.AppUser)
                    .Include(x => x.RestaurantSubscriptions)
                    .Where(entity => entity.AppUserId == userId)
                    .Select(x => Mapper.Map(x))
                    .ToListAsync())!;
        }

        public async Task<DalAppDTO.Restaurant?> GetRestaurantWithOrderData(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            return Mapper.Map(await query
                .AsNoTracking()
                .Include(x => x.AppUser)
                .Include(x => x.RestaurantOrder)
                .ThenInclude(x => x.FoodInOrders)
                .Where(x => x.AppUserId == userId && x.Id.Equals(id))
                .FirstOrDefaultAsync());
        }
    }
}