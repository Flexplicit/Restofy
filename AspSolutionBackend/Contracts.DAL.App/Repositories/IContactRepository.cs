using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface IContactRepository : IBaseRepository<DalAppDTO.Contact>,
        IContactCustomRepository<DalAppDTO.Contact>
    {
        Task<IEnumerable<DalAppDTO.Contact>> GetAllUserOrRestaurantContactsAsync(Guid userId, Guid? restaurantId,
            bool noTracking);
    }

    public interface IContactCustomRepository<TEntity>
    {
    }
}