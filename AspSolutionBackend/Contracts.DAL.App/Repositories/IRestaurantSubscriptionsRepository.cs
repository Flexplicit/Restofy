using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IRestaurantSubscriptionsRepository : IBaseRepository<DalAppDTO.RestaurantSubscription>
    {
    }

    public interface IRestaurantSubscriptionsCustomRepository<TEntity>
    {
    }
}