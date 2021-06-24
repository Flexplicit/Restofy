using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IOrderRepository : IBaseRepository<DalAppDTO.Order>, IOrderCustomRepository<DalAppDTO.Order>
    {
        Task<DalAppDTO.Order?> FirstOrDefaultUserOrRestaurant(Guid id, Guid userId = default, bool noTracking = true,
            bool restaurantUser = false);
    }

    public interface IOrderCustomRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllActiveOrdersAsync(Guid userId, bool restaurantOrders = false);
        
        Task<IEnumerable<TEntity>> GetAllUserOrdersAsync(Guid userId);
        
        Task<IEnumerable<TEntity>> GetAllRestaurantOrdersAsync(Guid userId);
        
        
        
    }
}