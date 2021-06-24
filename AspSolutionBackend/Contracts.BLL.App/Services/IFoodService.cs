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
    public interface IFoodService : IBaseEntityService<BLLAppDTO.Food, DALAppDTO.Food>,
        IFoodCustomRepository<BLLAppDTO.Food>
    {
        Task<IEnumerable<BLLAppDTO.Food>> GetRestaurantsFoodAsync(Guid restaurantId, bool noTracking = true);

        new Task<BLLAppDTO.Food> Remove(BLLAppDTO.Food entity, Guid userId = default);
    }
}