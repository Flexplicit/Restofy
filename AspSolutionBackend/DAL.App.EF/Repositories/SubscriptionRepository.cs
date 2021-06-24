using System;
using System.Collections.Generic;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;
    

namespace DAL.APP.EF.Repositories
{
    public class SubscriptionRepository: BaseRepository<DalAppDTO.Subscription,Domain.OrderModels.Subscription, AppDbContext>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext ctx, IMapper mapper) : base(ctx, new SubscriptionMapper(mapper))
        {
        }
        
    }
}