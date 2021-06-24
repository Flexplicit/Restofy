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
    public class BillRepository : BaseRepository<DalAppDTO.Bill, Domain.OrderModels.Bill, AppDbContext>, IBillRepository
    {
        public BillRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new BillMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DalAppDTO.Bill>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return (await query
                .Include(b => b.Order)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public override async Task<DalAppDTO.Bill?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return Mapper.Map(await query
                .Include(b => b.Order)
                .FirstOrDefaultAsync());
        }

        public async Task<DalAppDTO.Bill?> GetBillAccordingToOrderId(Guid orderId, Guid userId)
        {
            var query = RepoDbSet.AsQueryable();

            return Mapper.Map(await query
                .Include(x => x.Order)
                .ThenInclude(x => x!.Restaurant)
                .ThenInclude(x => x!.AppUser)
                .Where(x => x.Order!.Id.Equals(orderId))
                .Where(x => x.Order!.AppUserId.Equals(userId) || x.Order.Restaurant!.AppUserId.Equals(userId))
                .FirstOrDefaultAsync());
        }
    }
}