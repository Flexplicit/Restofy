using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;
using DomainDTO = Domain.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IFoodInOrderRepository : IBaseRepository<DalAppDTO.FoodInOrder>
    {
        IBaseMapper<DalAppDTO.FoodInOrder, DomainDTO.FoodInOrder> GetMapper();
        
        Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderCollectionByIdWithExtraInfoAsync(IEnumerable<Guid> foodInOrderIds);
        Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderCollectionByIdAsync(IEnumerable<Guid> foodInOrderIds);
        Task<IEnumerable<DalAppDTO.FoodInOrder>> GetFoodInOrderAccordingToOrderIdAsync(Guid orderId);
    }

    public interface IFoodInOrderCustomRepository<TEntity>
    {
    }
}