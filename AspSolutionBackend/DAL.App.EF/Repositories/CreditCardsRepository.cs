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
    public class CreditCardsRepository :
        BaseRepository<DalAppDTO.CreditCard, Domain.OrderModels.CreditCard, AppDbContext>, ICreditCardsRepository
    {
        public CreditCardsRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new CreditCardMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DalAppDTO.CreditCard>> GetAllAsync(Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return (await query
                .Include(u => u.AppUser)
                .Include(c => c.CreditCardInfo)
                .Where(x => x.AppUserId == userId)
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }

        public override async Task<DalAppDTO.CreditCard?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);


            return Mapper.Map(await query
                .Include(u => u.AppUser)
                .Include(c => c.CreditCardInfo)
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id) && entity.AppUserId.Equals(userId)));
        }
    }
}