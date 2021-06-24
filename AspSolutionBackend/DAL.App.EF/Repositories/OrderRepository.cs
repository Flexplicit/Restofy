using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.OrderModels;
using Domain.OrderModels.DbEnums;
using DalAppDTO = DAL.App.DTO.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories
{
    public class OrderRepository : BaseRepository<DalAppDTO.Order, Domain.OrderModels.Order, AppDbContext>,
        IOrderRepository
    {
        public OrderRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new OrderMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DalAppDTO.Order>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = RepoDbSet.AsQueryable();
            
            return (await query
                .Include(x => x.AppUser)
                .Include(x => x.Restaurant)
                .AsNoTracking()
                .Include(x => x.PaymentType)
                .Where(x => x.AppUserId.Equals(userId))
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public override async Task<DalAppDTO.Order?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = RepoDbSet.AsQueryable();// The one who orders and restaurant can access data

            return Mapper.Map(await query
                .AsNoTracking()
                .Include(x=>x.Restaurant)
                .Include(x => x.FoodInOrders)
                .ThenInclude(x => x.Food)
                .ThenInclude(x => x!.Cost)
                
                .Where(x=>x.AppUserId.Equals(userId) || x.Restaurant!.AppUserId.Equals(userId))
                .FirstOrDefaultAsync(e => e.Id.Equals(id)));
        }

        
        
        public async Task<DalAppDTO.Order?> FirstOrDefaultUserOrRestaurant(Guid id, Guid userId = default,
            bool noTracking = true, bool restaurantUser = false)
        {
            var query = RepoDbSet.AsQueryable();
            var firstDefault = (await query
                .Include(x => x.Restaurant)
                .Include(x => x.PaymentType)
                .Include(x => x.CreditCard)
                .Include(x => x.FoodInOrders)
                .AsNoTracking()
                .FirstOrDefaultAsync(entity =>
                    entity.Id.Equals(id)
                    && (restaurantUser
                        ? entity.Restaurant!.AppUserId.Equals(userId)
                        : entity.AppUserId.Equals(userId))));
            return Mapper.Map(firstDefault);
        }

        public async Task<IEnumerable<DalAppDTO.Order>> GetAllActiveOrdersAsync(Guid userId,
            bool restaurantOrders = false)
        {
            var query = CreateQuery(userId, false);

            var activeOrders = (await query
                    .Include(x => x.Restaurant)
                    .Include(x => x.FoodInOrders)
                    .ThenInclude(x => x.Food)
                    .ThenInclude(x => x!.Cost)
                    .Where(x => x.Restaurant!.AppUserId.Equals(userId) || x.AppUserId.Equals(userId))
                    .Where(x => x.OrderCompletionStatus != EOrderStatus.Finished)
                    .ToListAsync())
                .Select(x => Mapper.Map(x));
            return activeOrders!;
        }

        public async Task<IEnumerable<DalAppDTO.Order>> GetAllUserOrdersAsync(Guid userId)
        {
            var query = CreateQuery(userId, false);

            var userOrders = (await query
                    .Include(x => x.Restaurant)
                    .Include(x => x.FoodInOrders)
                    .ThenInclude(x => x.Food)
                    .ThenInclude(x => x!.Cost)
                    .Where(x => x.AppUserId.Equals(userId))
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync())
                .Select(x => Mapper.Map(x));
            return userOrders!;
        }

        public async Task<IEnumerable<DalAppDTO.Order>> GetAllRestaurantOrdersAsync(Guid userId)
        {

            var restaurantOrders = (await RepoDbSet.AsQueryable()
                    .Include(x => x.Restaurant)
                    .Include(x => x.FoodInOrders)
                    .ThenInclude(x => x.Food)
                    .ThenInclude(x => x!.Cost)
                    .Where(x => x.Restaurant!.AppUserId.Equals(userId))
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync())
                .Select(x => Mapper.Map(x));
            return restaurantOrders!;
        }

        public override DalAppDTO.Order Add(DalAppDTO.Order entity)
        {
            return Mapper.Map(RepoDbContext.Add(Mapper.Map(entity)!).Entity)!;
        }
    }
}