using System;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace DAL.APP.EF.Repositories
{
    public class CostRepository : BaseRepository<DalAppDTO.Cost, Domain.OrderModels.Cost, AppDbContext>, 
        ICostRepository
    {
        public CostRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new CostMapper(mapper))
        {
        }

        public override DalAppDTO.Cost Add(DalAppDTO.Cost entity)
        {
            Console.WriteLine(@"in cost Repository");
            return base.Add(entity);
        }
    }
}