using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IOrderService : IBaseEntityService<BLLAppDTO.Order, DALAppDTO.Order>,
        IOrderCustomRepository<BLLAppDTO.Order>
    {
        Task<BLLAppDTO.Order?> MakeAnOrderAsync(IEnumerable<Guid> orderItemsId, Guid appUserId, Guid paymentType, Guid? creditCardId);

        Task<BLLAppDTO.Order?> ConfirmOrderByRestaurantAsync(Guid orderId, int minutesTillReady,
            Guid restaurantWorkerId);

        new Task<BLLAppDTO.Order?> FirstOrDefaultAsync(Guid id, Guid userId, bool noTracking);

    }
}