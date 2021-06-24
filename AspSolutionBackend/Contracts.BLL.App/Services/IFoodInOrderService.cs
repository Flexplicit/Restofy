using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IFoodInOrderService : IBaseEntityService<BLLAppDTO.FoodInOrder, DALAppDTO.FoodInOrder>,
        IFoodInOrderCustomRepository<BLLAppDTO.FoodInOrder>
    {
        Task<IEnumerable<BLLAppDTO.FoodInOrder>> GetFoodInOrderAccordingToOrderIdAsync(Guid orderId);
    }
}