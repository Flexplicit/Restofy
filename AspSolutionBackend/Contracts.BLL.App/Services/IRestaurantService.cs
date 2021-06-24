using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IRestaurantService : IBaseEntityService<BLLAppDTO.Restaurant, DALAppDTO.Restaurant>,
        IRestaurantCustomRepository<BLLAppDTO.Restaurant>
    {
        Task<BLLAppDTO.Restaurant?> GetRestaurantWithMenuAsync(Guid id, Guid userId = default, bool noTracking = true);

        Task<IEnumerable<BLLAppDTO.Restaurant>> GetMyRestaurantsAsync(Guid userId = default, bool noTracking = true);

        new Task<BLLAppDTO.Restaurant> Remove(BLLAppDTO.Restaurant entity, Guid userId = default);

        new Task<IEnumerable<BLLAppDTO.Restaurant>> GetAllAsync(Guid userId, bool noTracking);

        new Task<BLLAppDTO.Restaurant?> FirstOrDefaultAsync(Guid id, Guid userId, bool noTracking);
    }
}