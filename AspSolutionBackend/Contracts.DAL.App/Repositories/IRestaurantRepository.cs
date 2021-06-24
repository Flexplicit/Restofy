using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IRestaurantRepository : IBaseRepository<DalAppDTO.Restaurant>

    {
        void GetTopRestaurants();

        Task<DalAppDTO.Restaurant?> GetRestaurantWithMenuAsync(Guid id, Guid userId = default, bool noTracking = true);

        Task<IEnumerable<DalAppDTO.Restaurant>?> GetMyRestaurants(Guid userId = default, bool noTracking = true);

        Task<DalAppDTO.Restaurant?> GetRestaurantWithOrderData(Guid id, Guid userId = default, bool noTracking = true);

        new DalAppDTO.Restaurant Remove(DalAppDTO.Restaurant entity, Guid userId = default);
    }

    public interface IRestaurantCustomRepository<TEntity>
    {
    }
}