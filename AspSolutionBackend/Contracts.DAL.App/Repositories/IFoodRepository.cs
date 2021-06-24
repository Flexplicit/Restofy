using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IFoodRepository : IBaseRepository<DalAppDTO.Food>, IFoodCustomRepository<DalAppDTO.Food>
    {
        Task<IEnumerable<DalAppDTO.Food>> GetRestaurantsFoodAsync(Guid restaurantId, bool noTracking = true);

    }

    public interface IFoodCustomRepository<TEntity>
    {
        Task<IEnumerable<DalAppDTO.Food>> GetFoodCollectionWithExtraInfoAsync(IEnumerable<Guid> foodItemsId);
    }
}