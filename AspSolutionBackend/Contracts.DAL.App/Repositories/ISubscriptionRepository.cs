using System;
using System.Collections.Generic;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface ISubscriptionRepository : IBaseRepository<DalAppDTO.Subscription>
    {
    }

    public interface ISubscriptionsCustomRepository<TEntity>
    {
    }
}