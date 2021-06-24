using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Mappers;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories
{
    public class FoodInOrderRepository :
        BaseRepository<DalAppDTO.FoodInOrder, Domain.OrderModels.FoodInOrder, AppDbContext>, IFoodInOrderRepository
    {
        // public IBaseMapper<DalAppDTO.FoodInOrder, Domain.OrderModels.FoodInOrder> SharedMapper { get; set; }

        public FoodInOrderRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new FoodInOrderMapper(mapper))
        {
            // SharedMapper = base.Mapper;
        }

        public override async Task<DalAppDTO.FoodInOrder?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return Mapper.Map(await query
                .Include(x => x.Food)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }

        public override async Task<IEnumerable<DalAppDTO.FoodInOrder>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return (await query
                .Include(x => x.Food)
                .Include(x => x.Order)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public IBaseMapper<DalAppDTO.FoodInOrder, Domain.OrderModels.FoodInOrder> GetMapper()
        {
            return Mapper;
        }

        public async Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderCollectionByIdWithExtraInfoAsync(
            IEnumerable<Guid> foodInOrderIds)
        {
            var query = CreateQuery(default, true);

            var dalFoodInOrderList = await query
                .Include(x => x.Food)
                .ThenInclude(x => x!.Cost)
                .Include(x => x.Food!.Cost)
                .Include(x => x.Food)
                .ThenInclude(x => x!.Restaurant)
                .Include(x => x.Food)
                .ThenInclude(x => x!.FoodGroup)
                .Where(x => foodInOrderIds.Contains(x.Id))
                .OrderByDescending(x => x.Food!.Cost!.CostWithVat)
                .Select(x => Mapper.Map(x))
                .ToListAsync();

            return dalFoodInOrderList!;
        }

        public async Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderAccordingToOrderIdAsync(Guid orderId)
        {
            var query = CreateQuery(default, true);

            return (await query
                .Include(x => x.Food)
                .ThenInclude(x => x!.Cost)
                .Include(x=>x.Food)
                .ThenInclude(x => x!.FoodGroup)
                .Include(x => x.Order)
                .Where(x => x.OrderId == orderId)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderCollectionByIdAsync(
            IEnumerable<Guid> foodInOrderIds)
        {
            throw new NotImplementedException();
        }
    }
}