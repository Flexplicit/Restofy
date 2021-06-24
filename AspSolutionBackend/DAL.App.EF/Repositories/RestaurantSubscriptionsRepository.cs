using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories
{
    public class RestaurantSubscriptionsRepository : BaseRepository<DalAppDTO.RestaurantSubscription, Domain.OrderModels.RestaurantSubscription, AppDbContext>,
        IRestaurantSubscriptionsRepository
    {
        public RestaurantSubscriptionsRepository(AppDbContext ctx, IMapper mapper) : base(ctx,
            new RestaurantSubscriptionMapper(mapper))
        {
        }

 

        public override async Task<IEnumerable<DalAppDTO.RestaurantSubscription>> GetAllAsync(Guid userId = default,
            bool noTracking = true)

        {
            var query = CreateQuery(userId, noTracking);


            return (await query
                .Include(x => x.Restaurant)
                .Include(x => x.Subscription)
                .Select(x=>Mapper.Map(x))
                .ToListAsync())!;
        }

        public override async Task<DalAppDTO.RestaurantSubscription?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)

        {
            var query = CreateQuery(userId, noTracking);


            return Mapper.Map(await query
                .Include(x => x.Restaurant)
                .Include(x => x.Subscription)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }
    }
}