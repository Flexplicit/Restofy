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
    public class ContactRepository : BaseRepository<DalAppDTO.Contact, Domain.OrderModels.Contact, AppDbContext>,
        IContactRepository
    {
        public ContactRepository(AppDbContext ctx, IMapper mapper) : base(ctx,
            new ContactMapper(mapper))
        {
        }


        public override async Task<DalAppDTO.Contact?> FirstOrDefaultAsync(Guid id, Guid userId = default,
            bool noTracking = true)
        {
            return Mapper.Map(await RepoDbSet.AsQueryable()
                .AsNoTracking()
                .Include(u => u.AppUser)
                .Include(r => r.Restaurant)
                .Include(ct => ct.ContactType)
                .FirstOrDefaultAsync(entity =>
                    entity.Id.Equals(id) &&
                    (entity.AppUserId.Equals(userId) || entity.Restaurant!.AppUserId.Equals(userId))));
        }

        // when restaurantId is null we give AppUser details and when resId isn't null we give res details.
        public async Task<IEnumerable<DalAppDTO.Contact>> GetAllUserOrRestaurantContactsAsync(Guid userId,
            Guid? restaurantId, bool noTracking)
        {
            var query = RepoDbSet.AsQueryable();
            return (await query
                .Include(u => u.AppUser)
                .Include(r => r.Restaurant)
                .Include(ct => ct.ContactType)
                .OrderByDescending(x => x.ContactType!.TypeName)
                .AsNoTracking()
                .Where(x => restaurantId == null
                    ? x.AppUserId.Equals(userId)
                    : x.RestaurantId.Equals(restaurantId))
                .Select(x => Mapper.Map(x))
                .ToListAsync())!;
        }
    }
}