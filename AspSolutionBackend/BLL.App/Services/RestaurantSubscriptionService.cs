using System;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class RestaurantSubscriptionService :
        BaseEntityService<IAppUnitOfWork, IRestaurantSubscriptionsRepository,
            BLLAppDTO.OrderModels.RestaurantSubscription,
            DALAppDTO.OrderModels.RestaurantSubscription, Guid>, IRestaurantSubscriptionService
    {
        public RestaurantSubscriptionService(IAppUnitOfWork serviceUow,
            IRestaurantSubscriptionsRepository serviceRepository,
            IMapper mapper) : base(
            serviceUow, serviceRepository, new RestaurantSubscriptionMapper(mapper))
        {
        }

        public new RestaurantSubscription Add(RestaurantSubscription entity)
        {
            var sub = ServiceUow.Subscriptions.FirstOrDefaultAsync(entity.SubscriptionId, default, true).Result;
            entity.ActiveSince = DateTime.Now;
            entity.ActiveUntill = DateTime.Now.AddDays(sub!.ValidDayCount);
            return Mapper.Map(ServiceRepository.Add(Mapper.Map(entity)!))!;
        }
    }
}