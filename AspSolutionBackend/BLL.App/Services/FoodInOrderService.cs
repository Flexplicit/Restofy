using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class FoodInOrderService :
        BaseEntityService<IAppUnitOfWork, IFoodInOrderRepository, BLLAppDTO.OrderModels.FoodInOrder,
            DALAppDTO.OrderModels.FoodInOrder, Guid>, IFoodInOrderService
    {
        public FoodInOrderService(IAppUnitOfWork serviceUow, IFoodInOrderRepository serviceRepository, IMapper mapper) :
            base(serviceUow, serviceRepository, new FoodInOrderMapper(mapper))
        {
        }

        public async Task<IEnumerable<FoodInOrder>> GetFoodInOrderAccordingToOrderIdAsync(Guid orderId)
        {
            return (await ServiceRepository.GetFoodInOrderAccordingToOrderIdAsync(orderId)).Select(x => Mapper.Map(x))!;
        }
    }
}