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
    public class FoodRepository : BaseRepository<DalAppDTO.Food, Domain.OrderModels.Food, AppDbContext>,
        IFoodRepository, IFoodCustomRepository<DalAppDTO.Food>
    {
        public FoodRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new FoodMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DalAppDTO.Food>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return (await query
                .Include(x => x.Cost)
                .Include(x => x.FoodGroup)
                .Include(x => x.Restaurant)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public override async Task<DalAppDTO.Food?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            return Mapper.Map(await query
                .Include(x => x.Cost)
                .Include(x => x.FoodGroup)
                .Include(x => x.Restaurant)
                .Include(x => x.FoodInOrders)
                .Where(entity => entity.Id.Equals(id))
                .AsNoTracking()
                .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<DalAppDTO.Food>> GetRestaurantsFoodAsync(Guid restaurantId,
            bool noTracking = true)
        {
            var query = CreateQuery(default, noTracking);

            var domainEntities = await query
                .Include(x => x.Cost)
                .Include(x => x.FoodGroup)
                .Where(x => x.RestaurantId.Equals(restaurantId))
                .OrderBy(x => x.FoodGroup!.FoodGroupType)
                .ToListAsync();
            var dalEntities = domainEntities.Select(x => Mapper.Map(x));
            return dalEntities!;
        }


        public async Task<IEnumerable<DalAppDTO.Food>> GetFoodCollectionWithExtraInfoAsync(
            IEnumerable<Guid> foodItemsId)
        {
            var query = CreateQuery(default, true);

            var dalFoodList = await query
                .Include(x => x.Cost)
                .Include(x => x.FoodGroup)
                .Include(x => x.Restaurant)
                .Where(x => foodItemsId.Contains(x.Id))
                .OrderByDescending(x => x.Cost!.CostWithVat)
                .Select(x => Mapper.Map(x))
                .ToListAsync();
            return dalFoodList!;
        }
    }
}