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


        public override async Task<DalAppDTO.Restaurant?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);
            var domainEntity = await query
                .Include(x => x.AppUser)
                .Include(x => x.RestaurantSubscriptions)
                .Include(x => x.NameLang)
                .ThenInclude(x => x!.Translations)
                .Include(x => x.DescriptionLang)
                .ThenInclude(x => x!.Translations)
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
                .Include(x => x.NameLang)
                .ThenInclude(x => x!.Translations)
                .Include(x => x.DescriptionLang)
                .ThenInclude(x => x!.Translations)
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
                .Include(x => x.NameLang)
                .ThenInclude(x => x!.Translations)
                .Include(x => x.DescriptionLang)
                .ThenInclude(x => x!.Translations)
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
                    .Include(x => x.NameLang)
                    .ThenInclude(x => x!.Translations)
                    .Include(x => x.DescriptionLang)
                    .ThenInclude(x => x!.Translations)
                    .Where(entity => entity.AppUserId == userId)
                    .Select(x => Mapper.Map(x))
                    .ToListAsync())!;
        }


        public override DalAppDTO.Restaurant Update(DalAppDTO.Restaurant entity)
        {
            var prevEntity = RepoDbSet
                .Include(x => x.NameLang)
                .ThenInclude(x => x!.Translations)
                .Include(x => x.DescriptionLang)
                .ThenInclude(x => x!.Translations)
                .FirstOrDefault(ent => ent.Id == entity.Id);
            
            prevEntity!.NameLang!.SetTranslation(entity.NameLang);
            prevEntity.DescriptionLang?.SetTranslation(entity.DescriptionLang!);
            
            var updatedEntity = RepoDbSet.Update(prevEntity!).Entity;
            var dalEntity = Mapper.Map(updatedEntity);
            return dalEntity!;
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
                .Include(x=>x.NameLang)
                .ThenInclude(x=>x!.Translations)
                .Where(x => x.AppUserId == userId && x.Id.Equals(id))
                .FirstOrDefaultAsync());
        }
    }
}