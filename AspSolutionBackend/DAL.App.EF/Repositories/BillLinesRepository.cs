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
    public class BillLinesRepository : BaseRepository<DalAppDTO.BillLine, Domain.OrderModels.BillLine, AppDbContext>,
        IBillLinesRepository
    {
        public BillLinesRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new BillLineMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DalAppDTO.BillLine>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            return (await query
                .Include(x => x.Bill)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }


        public override async Task<DalAppDTO.BillLine?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return Mapper.Map(await query
                .Include(x => x.Bill)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
        }

        public async Task<IEnumerable<DalAppDTO.BillLine>> GetAllBillLinesAccordingToBillId(Guid userId, Guid billId)
        {
            //TODO: maybe auth for userid, might not be necessary since it happens on bill anyways
            var query = RepoDbSet.AsQueryable();
            return (await query
                .Include(x => x.Bill)
                .Where(x => x.Bill.Id.Equals(billId))
                .ToListAsync()).Select(x => Mapper.Map(x))!;
        }
    }
}